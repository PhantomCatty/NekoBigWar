using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//这个脚本是用来获取目标的,它不执行使武器指向obj的操作
//对于EnemyAimDetector而言,除了提供currenttarget,还会提供followtarget给enemycontroller
public class EnemyAimDetector : AgentAimDetector
{

    public GameObject followTarget;

    public TargetType followPrefer1;
    public TargetType followPreferDefault;
    //public TargetType aimPrefer1;
    //public TargetType aimPrefer2;

    void Start()
    {
        calculateList = new List<float>();
        targetList = new List<GameObject>();
        detectTimer = 0f;
        player = InGameData.instance.player;
        findFollow();
    }

    void Update()
    {
        if (detectTimer < detectInterval) detectTimer += Time.deltaTime;
        if (detectTimer > detectInterval) { findTarget(); detectTimer = 0f; }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool flag = false;
        GameObject oobj = other.gameObject;
        if (other.tag == "Enemy" && CommonTools.compareTarget(targetType, TargetType.ENEMY))
        {
            flag = true;
            if (targetList.Count == 0)
            {
                currentTarget = oobj;
            }
            if (!targetList.Find(obj => obj == oobj))
                targetList.Add(oobj);
        }
        else if ((other.tag == "Agent") && CommonTools.compareTarget(targetType, TargetType.ALLY))
        {
            flag = true;
            if (targetList.Count == 0)
            {
                currentTarget = oobj;
            }
            if (!targetList.Find(obj => obj == oobj))
                targetList.Add(oobj);
        }
        else if ((other.tag == "Enemy_Fortification" || other.tag == "Enemy_Base") && CommonTools.compareTarget(targetType, TargetType.ENEMY_FORTIFICATION))
        {
            flag = true;
            if (targetList.Count == 0)
            {
                currentTarget = oobj;
            }
            if (!targetList.Find(obj => obj == oobj))
                targetList.Add(oobj);
        }
        else if ((other.tag == "Ally_Fortification"||other.tag == "Ally_Base") && CommonTools.compareTarget(targetType, TargetType.ALLY_FORTIFICATION))
        {
            flag = true;
            if (targetList.Count == 0)
            {
                currentTarget = oobj;
            }
            if (!targetList.Find(obj => obj == oobj))
                targetList.Add(oobj);
        }
        if (flag==true)
        {
            findFollow();
            findTarget();
            //if (priorIndex(oobj) > 0)
            //{
            //    findFollow();
            //    //targetList.Remove(oobj);
            //    //targetList.Insert(0, oobj);
            //}
            //else if (followPrefer1 <= 0)//如果不是优先目标,和currenttarget比较
            //{
            //    if (priorIndex(currentTarget) > 0)
            //    {
            //        targetList.Remove
            //        targetList.Insert(0, oobj);
            //    }
            //    findFollow(currentTarget);
            //}
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
        if (followTarget == other.gameObject)
        {
            findFollow();
        }
    }

    public void findFollow()
    {
        // 优先目标为Base时，直接用Base列表
        if (followPrefer1 == TargetType.BASE && InGameData.instance.allyBaseList.Count > 0)
        {
            float minDist = float.MaxValue;
            GameObject nearestBase = null;
            foreach (var baseObj in InGameData.instance.allyBaseList)
            {
                float dist = (baseObj.transform.position - transform.position).magnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestBase = baseObj;
                }
            }
            followTarget = nearestBase;
            return;
        }

        List<GameObject> preferList = new List<GameObject>();

        // 1. 先筛选所有符合followPrefer1类型的目标
        foreach (var obj in targetList)
        {
            string tag = obj.tag;
            TargetType type = TargetType.NONE;
            switch (tag)
            {
                case "Player": type = TargetType.PLAYER; break;
                case "Agent": type = TargetType.ALLY; break;
                case "Enemy": type = TargetType.ENEMY; break;
                case "Ally_Base": type = TargetType.BASE; break;
                case "Ally_Fortification": type = TargetType.ALLY_FORTIFICATION; break;
                case "Enemy_Fortification": type = TargetType.ENEMY_FORTIFICATION; break;
                default: break;
            }
            if (type == followPrefer1)
            {
                preferList.Add(obj);
            }
        }

        // 2. 如果有优先目标，根据AimingStrategy选择一个
        if (preferList.Count > 0)
        {
            switch (aimMethod)
            {
                case AimingStrategy.FIRST:
                    followTarget = preferList[0];
                    break;
                case AimingStrategy.LAST:
                    followTarget = preferList[preferList.Count - 1];
                    break;
                case AimingStrategy.NEAREST_TO_SELF:
                    {
                        float minDist = float.MaxValue;
                        GameObject nearest = null;
                        foreach (var obj in preferList)
                        {
                            float dist = (obj.transform.position - transform.position).magnitude;
                            if (dist < minDist)
                            {
                                minDist = dist;
                                nearest = obj;
                            }
                        }
                        followTarget = nearest;
                    }
                    break;
                case AimingStrategy.LESS_HP:
                    {
                        float minHpRatio = float.MaxValue;
                        GameObject weakest = null;
                        foreach (var obj in preferList)
                        {
                            float hpRatio = 1f;
                            if (obj.CompareTag("Enemy"))
                            {
                                var ec = obj.GetComponentInChildren<EnemyController>();
                                if (ec != null) hpRatio = ec.currentHP / ec.enemyBasic.maxHP;
                            }
                            else if (obj.CompareTag("Agent"))
                            {
                                var ac = obj.GetComponentInChildren<AgentController>();
                                if (ac != null) hpRatio = ac.currentHP / ac.agentBasic.maxHP;
                            }
                            else if (obj.CompareTag("Ally_Fortification") || obj.CompareTag("Enemy_Fortification"))
                            {
                                var fc = obj.GetComponentInChildren<FortificationController>();
                                if (fc != null) hpRatio = fc.currentHP / fc.fortiBasic.maxHP;
                            }
                            else if (obj.CompareTag("Ally_Base"))
                            {
                                var ac = obj.GetComponentInChildren<BaseController>();
                                if (ac != null) hpRatio = ac.currentHP / ac.baseBasic.maxHP;
                            }
                            if (hpRatio < minHpRatio)
                            {
                                minHpRatio = hpRatio;
                                weakest = obj;
                            }
                        }
                        followTarget = weakest;
                    }
                    break;
                default:
                    followTarget = preferList[0];
                    break;
            }
            return;
        }

        // 默认目标为Base时，直接用Base列表
        if (followPreferDefault == TargetType.BASE && InGameData.instance.allyBaseList.Count > 0)
        {
            float minDist = float.MaxValue;
            GameObject nearestBase = null;
            foreach (var baseObj in InGameData.instance.allyBaseList)
            {
                float dist = (baseObj.transform.position - transform.position).magnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestBase = baseObj;
                }
            }
            followTarget = nearestBase;
            return;
        }

        // 3. 没有优先目标，根据followPreferDefault选择
        foreach (var obj in targetList)
        {
            string tag = obj.tag;
            TargetType type = TargetType.NONE;
            switch (tag)
            {
                case "Player": type = TargetType.PLAYER; break;
                case "Agent": type = TargetType.ALLY; break;
                case "Enemy": type = TargetType.ENEMY; break;
                case "Ally_Base": type = TargetType.BASE; break;
                case "Ally_Fortification": type = TargetType.ALLY_FORTIFICATION; break;
                case "Enemy_Fortification": type = TargetType.ENEMY_FORTIFICATION; break;
                default: break;
            }
            if (type == followPreferDefault)
            {
                followTarget = obj;
                return;
            }
        }

        // 4. 如果还没有，默认跟随当前目标或第一个基地
        if (currentTarget)
            followTarget = currentTarget;
        else if (InGameData.instance.allyBaseList.Count > 0)
            followTarget = InGameData.instance.allyBaseList[0];
        else
            followTarget = null;
    }


    //根据偏好找follow对象,如果侦测目标里有偏好对象会带个参数,那就直接选带的参数对象了
    //public void findFollow()
    //{
    //    if (followPrefer1.GetHashCode() != 0)
    //    {
    //        //According to current code, 
    //        switch (followPrefer1)
    //        {
    //            case TargetType.ALLY:
    //            case TargetType.PLAYER:
    //            //followTarget = player;
    //            //return;
    //            case TargetType.ALLY_FORTIFICATION:
    //            case TargetType.BASE:
    //                List<GameObject> list = InGameData.instance.allyBaseList;
    //                List<float> distList = new List<float>();
    //                for (int i = 0; i < list.Count; i++)
    //                {
    //                    distList.Add(Vector3.Magnitude(transform.position - list[i].transform.position));
    //                }
    //                int idx = 0;
    //                for (int i = 0; i < distList.Count; i++)
    //                {
    //                    if (distList[idx] > distList[i])
    //                    {
    //                        idx = i;
    //                    }
    //                }
    //                followTarget = list[idx];
    //                return;
    //            default:
    //                followTarget = currentTarget;
    //                return;
    //        }
    //    }
    //    else if (currentTarget)
    //        followTarget = currentTarget;
    //    else
    //        followTarget = InGameData.instance.allyBaseList[0];
    //    return;
    //}
    public void findFollow(GameObject target)
    {
        followTarget = target;
    }

    /// <summary>
    /// 就是返回当前目标是不是优先选择目标,如果是返回编号1/2表示优先级,否则返回-1
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public int priorIndex(GameObject target)
    {
        string tag = target.tag;
        TargetType type;
        switch (tag)
        {
            case "Player":
                type = TargetType.PLAYER;
                break;
            case "Agent":
                type = TargetType.ALLY;
                break;
            case "Enemy":
                type = TargetType.ENEMY;
                break;
            case "Ally_Base":
                type = TargetType.BASE;
                break;
            case "Ally_Fortification":
                type = TargetType.ALLY_FORTIFICATION;
                break;
            case "Enemy_Fortification":
                type = TargetType.ENEMY_FORTIFICATION;
                break;
            default: return -1;
        }
        if (followPrefer1 == type)
        {
            return 1;
        }
        //else if (followPrefer2 == type)
        //{
        //    return 2;
        //}
        else return -1;
    }

    //No argument, this is designed as callback func;
    /// <summary>
    /// 先找优先级,后找偏好
    /// </summary>
    protected override void findTarget()
    {
        bool flag = false;
        string tag = "";
        if (followPrefer1 != 0)
        {
            switch (followPrefer1)
            {
                case TargetType.ALLY:
                    tag = "Agent";
                    break;
                case TargetType.PLAYER:
                    tag = "Player";
                    break;
                case TargetType.ALLY_FORTIFICATION:
                    tag = "Ally_Fortification";
                    break;
                case TargetType.BASE:
                    tag = "Ally_Base";
                    break;
                default:break;
            }
        }
        GameObject temp = targetList.Find((obj => obj.tag == tag));
        if (temp)
        {
            flag = true;
            currentTarget = temp;
            followTarget = temp;
        }
        if (flag == false)
        {
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
                                calculateList.Add((obj.transform.position - player.transform.position).magnitude);//wrong,not 'player' but 'base'
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
        }

    }

    //when should findTarget called? Three situations:
    //current target is lost;
    //first enemy is in the range;
    //timely reset;
}