using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.AI;

/// <summary>
/// UIControllerֻ�ṩ��UI��ͼ�����Ľӿ�,�õ�����������������
/// </summary>
public class UIControllerInGame : MonoBehaviour
{
    public static UIControllerInGame instance;

    public Image HPBar;
    public Image AmmoBar;
    public Image FrontSight;//���㼼�ܵ�׼��
    public Image AimArea;//��Χ���ܵķ�Χָʾ��
    public TextMeshProUGUI CostLabel;
    public TextMeshProUGUI KillsLabel;
    //����д�϶�������,�����ɰ�scriptableObject�Ź���,
    public List<GameObject> agentList;//��Ŀ���ŵĸ�Աԭ��(��������sobj),Ҳ����Ԥ����
    public List<GameObject> agentSpriteList;//��Ա��sprite,ֻ�������
    public List<SlotItem> slotList;//��λ�������б�

    public int curIndex;

    private bool isPlacing;
    private bool isUsingSkill;
    private AgentSkill curSkill;

    public SkillEffectDetector SkillAimDetector;//����Ŀ��ѡ����,ֻ������Ҫѡ���ض���λ��ʱ��,�񳣹�detectorһ��,��һ���Ƚϴ�ķ�Χ��ѡ��Ŀ��
    public CircleCollider2D circle;

    private UnityAction<GameObject> inAction;
    private UnityAction<GameObject> outAction;

    private void Awake()
    {
        circle = SkillAimDetector.GetComponent<CircleCollider2D>();
        circle.radius = 1f;
        SkillAimDetector.detectInterval = 0.01f;
        SkillAimDetector.gameObject.SetActive(false);
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        isPlacing = false;
        isUsingSkill = false;
        HPBar.fillAmount = 1;
        AmmoBar.fillAmount = 1;
        for (int i = 0; i < 8; i++)
        {
            agentSpriteList.Add(Instantiate(agentList[i].transform.GetChild(1).gameObject));
            agentSpriteList[i].GetComponent<SpriteRenderer>().sortingOrder = 40;
            agentSpriteList[i].GetComponent<Animator>().enabled = false;
            agentSpriteList[i].SetActive(false);
            Destroy(agentSpriteList[i].GetComponent<Collider2D>());
            //slotlist�������ⲿֱ��ָ��
            //slotList[i] = transform.GetChild(1).GetChild(i).GetComponent<SlotItem>();
            slotList[i].agentInstance = agentList[i];
        }
        FrontSight.gameObject.SetActive(false);
        AimArea.gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        //KillsLabel.text = "Kills: 0";
        inAction += InfoCanvas.instance.addFrontSight;
        outAction += InfoCanvas.instance.removeFrontSight;
    }

    // Update is called once per frame
    void Update()
    {
        //the icon of agents follows the cursor 
        if (isPlacing == true)
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec.z = 0f;
            agentSpriteList[curIndex].transform.position = vec;
        }
        //�������ʹ�ü���״̬,���ݼ������ͼ���׼�Ǻͷ�Χָʾ��,����
        if (isUsingSkill == true)
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec.z = 0f;
            if (SkillAimDetector.gameObject.activeSelf == true && SkillAimDetector.currentTarget)
            {
                FrontSight.transform.position = SkillAimDetector.currentTarget.transform.position;
            }
            else FrontSight.transform.position = vec;
            if (AimArea.gameObject.activeSelf == true) AimArea.transform.position = vec;
            if (SkillAimDetector.gameObject.activeSelf == true) SkillAimDetector.transform.position = vec;
        }
    }

    public void setHPBar(float curHP, float maxHP)
    {
        HPBar.fillAmount = curHP / maxHP;
    }

    public void setAmmoBar(float curAmmo, float maxAmmo)
    {
        AmmoBar.fillAmount = curAmmo / maxAmmo;
    }

    public void setCostLabel(float cost)
    {
        CostLabel.text = "Cost: "+Mathf.Floor(cost);
    }

    public void setKillsLabel(int kills)
    {
        //KillsLabel.text = "Kills: " + kills;
    }

    /// <summary>
    /// �¼����õķ���:����Ա�����µ�ʱ��,����һ��sprite,������ҵ�������λ��.��������ƶ���,��������϶�ʱ����
    /// </summary>
    /// <param name="index">
    /// ��Ա���ĵڼ����������(0-7)
    /// </param>
    public void onAgentIconClick(int index)
    {
        //�ظ��������ȡ��ʩ�ż���/���ø�Ա;
        if (isPlacing == true)
        {
            isPlacing = false;
            agentSpriteList[curIndex].SetActive(false);
            if (curIndex == index)
            {
                curIndex = -1;
                return;
            }
        }
        if (isUsingSkill == true)
        {
            isUsingSkill = false;
            InGameController.instance.RecoverSpeed();
            FrontSight.gameObject.SetActive(false);
            AimArea.gameObject.SetActive(false);
            SkillAimDetector.gameObject.SetActive(false);
            if (curIndex == index)
            {
                curIndex = -1;
                return;
            }
        }

        if (slotList[index].slotStatus == SlotStatus.AGENT)
        {
            if (isPlacing == false)
            {
                isPlacing = true;
                curIndex = index;
                agentSpriteList[index].SetActive(true);
            }
            else
            {
                if (curIndex != index)
                {
                    agentSpriteList[curIndex].SetActive(false);
                    curIndex = index;
                    agentSpriteList[index].SetActive(true);
                }
                else
                {
                    isPlacing = false;
                    agentSpriteList[index].SetActive(false);
                }

            }
        }
        else if (slotList[index].slotStatus == SlotStatus.SKILL)//����icon
        {
            curSkill = InGameData.instance.agentSkillList[index];
            isUsingSkill = true;
            InGameController.instance.Slow();
            curIndex = index;
            FrontSight.gameObject.SetActive(true);
            if (curSkill.compareExertType(SkillExertType.SELF)) return ;//SELF״̬�²�ʹ�������
            else if (curSkill.compareExertType(SkillExertType.POSITION)) 
            {
                //POSITIONģʽ��,����ʹ��ָʾ��,����ָʾ��������������׼��Χ�ڵĵ�λ(����,��������⵽�ĵ�λ),�����������������ܵĲ���
                AimArea.gameObject.SetActive(true);
                AimArea.transform.localScale = new Vector3(curSkill.skillBasic.areaRadius, curSkill.skillBasic.areaRadius, 1);
                SkillAimDetector.gameObject.SetActive(true);
                if(curSkill.skillBasic.areaRadius==0f) SkillAimDetector.gameObject.SetActive(false);
                else SkillAimDetector.GetComponent<CircleCollider2D>().radius = curSkill.skillBasic.areaRadius;

                if ((curSkill.skillBasic.effectType & SkillEffectType.ALLY) == SkillEffectType.ALLY) SkillAimDetector.targetType |= TargetType.ALLY;
                if ( (curSkill.skillBasic.effectType & SkillEffectType.ENEMY) == SkillEffectType.ENEMY) SkillAimDetector.targetType |= TargetType.ENEMY;
                if ( (curSkill.skillBasic.effectType & SkillEffectType.ENEMY_FORTIFICATION) == SkillEffectType.ENEMY_FORTIFICATION) SkillAimDetector.targetType |= TargetType.ENEMY_FORTIFICATION;
                if ( (curSkill.skillBasic.effectType & SkillEffectType.ALLY_FORTIFICATION) == SkillEffectType.ALLY_FORTIFICATION) SkillAimDetector.targetType |= TargetType.ALLY_FORTIFICATION;
                SkillAimDetector.targetInEvent.AddListener(inAction);
                SkillAimDetector.targetOutEvent.AddListener(outAction);
                //���Ƿ�Χ����,��ʹ��ָʾ��,����ʾ��ΧUI
            }
            else//ALLY/ENEMY/FORTIFICATION����ʹ��ָʾ��,��ʹ�÷�ΧUI,Ŀǰֻ���ص�һ��Ŀ��(��С������ѡ�񼸸�Ŀ��,����"SELF")
            {
                SkillAimDetector.gameObject.SetActive(true);
                SkillAimDetector.targetType = 0;
                if (curSkill.compareExertType(SkillExertType.ALLY)) SkillAimDetector.targetType |= TargetType.ALLY;
                if (curSkill.compareExertType(SkillExertType.ENEMY)) SkillAimDetector.targetType |= TargetType.ENEMY;
                if (curSkill.compareExertType(SkillExertType.ENEMY_FORTIFICATION)) SkillAimDetector.targetType |= TargetType.ENEMY_FORTIFICATION;
                if (curSkill.compareExertType(SkillExertType.ALLY_FORTIFICATION)) SkillAimDetector.targetType |= TargetType.ALLY_FORTIFICATION;
                SkillAimDetector.targetInEvent.RemoveAllListeners();
                SkillAimDetector.targetOutEvent.RemoveAllListeners();
            }
        }
    }

    /// <summary>
    /// ���¸�Աʱ�����õķ���,������ú�����������õ�����
    /// </summary>
    public void onPlacing()
    {
        if (isPlacing == true)
        {
            float cost = slotList[curIndex].curObj.agentBasic.AssignCost;
            if (InGameData.instance.curCost > cost)
            {
                isPlacing = false;
                agentSpriteList[curIndex].SetActive(false);
                GameObject temp = Instantiate(agentList[curIndex]);
                temp.GetComponentInChildren<AgentController>().index = curIndex;
                Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                vec.z = 0f;
                temp.transform.position = vec;
                temp.GetComponentInChildren<NavMeshAgent>().Warp(vec);
                InGameData.instance.agentList[curIndex] = temp;
                curSkill = temp.GetComponentInChildren<AgentSkill>();
                InGameData.instance.agentSkillList[curIndex] = curSkill;
                slotList[curIndex].setSlotStatus(SlotStatus.SKILL);
                InGameData.instance.addCost(-cost);
                InfoCanvas.instance.addHPBar(temp, TargetType.ALLY);
            }
        }
        else if (isUsingSkill == true)
        {
            float cost = slotList[curIndex].curObj.agentBasic.SkillCost;
            if(InGameData.instance.curCost > cost)
            {
                isUsingSkill = false;
                InGameController.instance.RecoverSpeed();
                FrontSight.gameObject.SetActive(false);
                AimArea.gameObject.SetActive(false);
                if (curSkill.compareExertType(SkillExertType.SELF))
                {
                    curSkill.releaseSkill(new List<GameObject> { curSkill.transform.parent.gameObject });
                }
                else if (curSkill.compareExertType(SkillExertType.POSITION))
                {
                    Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    vec.z = 0f;
                    curSkill.releaseSkill(vec);
                }
                else
                {
                    if (SkillAimDetector.targetList.Count == 0)//ѡ��Ŀ��,����û��Ŀ��,���ܲ���Ч
                    {
                        SkillAimDetector.gameObject.SetActive(false);
                        return;
                    }
                    curSkill.releaseSkill(SkillAimDetector.targetList);
                }
                slotList[curIndex].setSlotStatus(SlotStatus.SKILL_CD);
                InGameData.instance.addCost(-cost);
                SkillAimDetector.gameObject.SetActive(false);
            }
        }
    }

    public void agentDestoyed(int index)
    {
        if(index==curIndex && isUsingSkill == true)
        {
            isUsingSkill = false;
            InGameController.instance.RecoverSpeed();
            FrontSight.gameObject.SetActive(false);
            AimArea.gameObject.SetActive(false);
            SkillAimDetector.gameObject.SetActive(false);
            InfoCanvas.instance.clearFrontSight();
            if (curIndex == index)
            {
                curIndex = -1;
                return;
            }
        }
    }
}