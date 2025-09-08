using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//����ű���������ȡĿ���,����ִ��ʹ����ָ��obj�Ĳ���
public class AgentAimDetector : MonoBehaviour
{
    [HideInInspector]
    public float detectInterval=0.2f;
    public List<GameObject> targetList;
    public GameObject currentTarget;
    [Header("高级索敌：目标锁定，开启后只要当前目标存在就不会切换目标")]
    public bool lockTarget = false;
    public AimingStrategy aimMethod;
    protected float detectTimer;
    protected List<float> calculateList;
    protected GameObject player;
    [MultiEnum] public TargetType targetType;

    [SerializeField]
    private GameObject lastTarget;

    public UnityEvent aimChangeEvent;

    void Start()
    {
        calculateList = new List<float>();
        targetList = new List<GameObject>();
        detectTimer = 0f;
        player = InGameData.instance.player;
        lastTarget = null;
    }

    void Update()
    {
        if(detectTimer<detectInterval) detectTimer += Time.deltaTime;
        if (detectTimer > detectInterval) { findTarget(); detectTimer = 0f; }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && CommonTools.compareTarget(targetType, TargetType.ENEMY))
        {
            if (targetList.Count == 0)
            {
                currentTarget = other.gameObject;
            }
            if (!targetList.Find(obj => obj == other.gameObject))
                targetList.Add(other.gameObject);
        }
        else if ((other.tag == "Agent" || other.tag == "Player") && CommonTools.compareTarget(targetType, TargetType.ALLY))
        {
            if (targetList.Count == 0)
            {
                currentTarget = other.gameObject;
            }
            if (!targetList.Find(obj => obj == other.gameObject))
                targetList.Add(other.gameObject);
        }
        else if (other.tag == "Enemy_Fortification" && CommonTools.compareTarget(targetType, TargetType.ENEMY_FORTIFICATION))
        {
            if (targetList.Count == 0)
            {
                currentTarget = other.gameObject;
            }
            if (!targetList.Find(obj => obj == other.gameObject))
                targetList.Add(other.gameObject);
        }
        else if (other.tag == "Ally_Fortification" && CommonTools.compareTarget(targetType, TargetType.ALLY_FORTIFICATION))
        {
            if (targetList.Count == 0)
            {
                currentTarget = other.gameObject;
            }
            if (!targetList.Find(obj => obj == other.gameObject))
                targetList.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //procedure:find obj in the list, remove it;
        targetList.Remove(other.gameObject);
        if (currentTarget == other.gameObject)
        {
            findTarget();
        }
    }

    //No argument, this is designed as callback func;
    protected virtual void findTarget()
    {
        // 目标锁定：只在当前目标丢失时才重新选择，否则一直锁定当前目标
        if (lockTarget && currentTarget != null && targetList.Contains(currentTarget))
        {
            if (lastTarget != currentTarget)
            {
                aimChangeEvent.Invoke();
                lastTarget = currentTarget;
            }
            return;
        }

        switch (aimMethod)
        {
            case AimingStrategy.FIRST:
                {
                    if (targetList.Count == 0)
                    {
                        currentTarget = null;
                    }
                    else currentTarget = targetList[0];
                    break;
                }
            case AimingStrategy.LAST:
                {
                    if (targetList.Count == 0)
                    {
                        currentTarget = null;
                    }
                    //here should have a better way but I'm unfamiliar with API of List;
                    else currentTarget = targetList[targetList.Count - 1];
                    break;
                }
            case AimingStrategy.NEAREST_TO_BASE:
                {
                    if (targetList.Count == 0)
                    {
                        currentTarget = null;
                    }
                    else
                    {
                        calculateList.Clear();
                        foreach (GameObject obj in targetList)
                        {
                            calculateList.Add((obj.transform.position - InGameData.instance.allyBaseList[0].transform.position).magnitude);//wrong,not 'player' but 'base'
                        }
                        int idx = 0;
                        for (int i = 0; i < calculateList.Count; i++)
                        {
                            if (calculateList[idx] > calculateList[i])
                            {
                                idx = i;
                            }
                        }
                        currentTarget = targetList[idx];

                    }
                    break;
                }
            case AimingStrategy.NEAREST_TO_PLAYER:
                {
                    if (targetList.Count == 0)
                    {
                        currentTarget = null;
                    }
                    else
                    {
                        calculateList.Clear();
                        foreach (GameObject obj in targetList)
                        {
                            calculateList.Add((obj.transform.position - player.transform.position).magnitude);
                        }
                        int idx = 0;
                        for (int i = 0; i < calculateList.Count; i++)
                        {
                            if (calculateList[idx] > calculateList[i])
                            {
                                idx = i;
                            }
                        }
                        currentTarget = targetList[idx];
                    }
                    break;
                }
            case AimingStrategy.NEAREST_TO_SELF:
                {
                    if (targetList.Count == 0)
                    {
                        currentTarget = null;
                    }
                    else
                    {
                        calculateList.Clear();
                        foreach (GameObject obj in targetList)
                        {
                            calculateList.Add((obj.transform.position - transform.position).magnitude);
                        }
                        int idx = 0;
                        for (int i = 0; i < calculateList.Count; i++)
                        {
                            if (calculateList[idx] > calculateList[i])
                            {
                                idx = i;
                            }
                        }
                        currentTarget = targetList[idx];
                    }
                    break;
                }
            case AimingStrategy.LESS_HP:
                {
                    if (targetList.Count == 0)
                    {
                        currentTarget = null;
                    }
                    else
                    {
                        calculateList.Clear();
                        foreach (GameObject obj in targetList)
                        {
                            switch (obj.tag)
                            {
                                case "Enemy":
                                    {
                                        EnemyController eCont = obj.GetComponentInChildren<EnemyController>();
                                        calculateList.Add(eCont.currentHP / eCont.enemyBasic.maxHP);
                                        break;
                                    }
                                case "Player":
                                    {
                                        NekoBasic eCont = obj.GetComponentInChildren<NekoBasic>();
                                        calculateList.Add(eCont.currentHp / eCont.maxHP);
                                        break;
                                    }
                                case "Agent":
                                    {
                                        AgentController eCont = obj.GetComponentInChildren<AgentController>();
                                        calculateList.Add(eCont.currentHP / eCont.agentBasic.maxHP);
                                        break;
                                    }
                                case "Fortification":
                                    {
                                        break;
                                    }
                                default: break;
                            }
                        }
                        int idx = 0;
                        for (int i = 0; i < calculateList.Count; i++)
                        {
                            if (calculateList[idx] > calculateList[i])
                            {
                                idx = i;
                            }
                        }
                        currentTarget = targetList[idx];
                    }
                    break;
                }
            default: break;
        }

        if (lastTarget != currentTarget)
        {
            aimChangeEvent.Invoke();
            lastTarget = currentTarget;
        }
    }

    //when should findTarget called? Three situations:
    //current target is lost;
    //first enemy is in the range;
    //timely reset;
}

//TargetType decides which kinds of obj will be detected
[System.Flags]
public enum TargetType
{
    ENEMY = 1,
    ALLY = 2,
    ALLY_FORTIFICATION = 4,//is base included?
    ENEMY_FORTIFICATION = 8,
    TARGET = 16,
    BASE = 32,//��Щ�ط�Ҫ��base��fortification�ֿ�����,��������»��õ����Ŷ
    PLAYER = 64,//playerͬ��
    NONE = 128,
}

public class MultiEnum : PropertyAttribute
{

}

public enum AimingStrategy
{
    FIRST = 0,
    LAST = 1,
    NEAREST_TO_SELF = 2,
    NEAREST_TO_BASE = 3,
    NEAREST_TO_PLAYER = 4,
    //when talking about 'strongest', the standard should be figured
    //the one with most HP is ok and easy to conduct, but is that strongest?
    //to calculate or preset a value is ideal but hard to conduct and cost-consuming
    STRONGEST = 5,
    WEAKEST = 6,
    FURTHER_TO_SELF = 7,
    ALLY_IN_DANGER=8,
    LESS_HP=9,
}
