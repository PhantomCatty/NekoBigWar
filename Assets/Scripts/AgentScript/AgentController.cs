using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using UnityEngine.AI;

public enum AgentState
{
    Idle,
    Move,
    Attack,
    Dead
}

public class AgentController : BaseAnimEventController
{
    [HideInInspector]
    public int index;
    [SerializeField]
    public AgentBasic agentBasic;

    public float currentHP;
    public int currentAmmo;
    public float curSkillCD;

    public float maxVelocity;
    public float currVelocity;
    public float acceleration;


    public bool isleft;
    Transform weapontr;
    [HideInInspector]
    public FireController fireController;
    public AgentAimDetector detector;
    CircleCollider2D detectCircle;
    [HideInInspector]
    public GameObject player;
    public BuffCalculator calculator;

    public Vector3 point;

    [Header("ji ji jimo jimo~")]
    public UnityEvent statusChangeEvent;

    private NavMeshAgent naviAgent;
    private Animator animator;
    public AgentState currentState;
    private bool isAttacking = false; // 攻击动画播放中

    public float standHoldTimer;
    public float standHoldThreshold = 3f;
    bool isInNormal;

    // Start is called before the first frame update
    void Start()
    {
        calculator = GetComponent<BuffCalculator>();
        agentBasic = GetComponent<AgentBasic>();
        currentHP = agentBasic.maxHP;
        detectCircle = GetComponentInChildren<CircleCollider2D>();
        detectCircle.radius = agentBasic.detectRange;
        detector = GetComponentInChildren<AgentAimDetector>();
        fireController = GetComponentInChildren<FireController>();
        fireController.calculator = calculator;
        weapontr = transform.Find("WeaponPlace").transform;
        player = InGameData.instance.player;
        detector.targetType = agentBasic.targetType;
        //agentBasic.setWeapon();
        if (agentBasic.agentType == AgentType.SPINER)
        {
            //agentBasic.range *= 1.5f;
            //agentBasic.damage *= 1.2f;
            calculator.addParam(CalculateItem.RANGE_FINALM, 0.5f);
            calculator.addParam(CalculateItem.ATTACK_FINALM, 0.2f);
            agentBasic.setRange();
            agentBasic.setDamage();
        }
        else
        {
            //Debug.Log("not sniper");
            //agentBasic.range *= 1.3f;
            calculator.addParam(CalculateItem.RANGE_FINALM, 0.3f);
            agentBasic.setRange();
        }
        setDetectRange(agentBasic.range);

        naviAgent = GetComponent<NavMeshAgent>();
        naviAgent.updateRotation = false;
        naviAgent.updatePosition = false;
        naviAgent.updateUpAxis = false;
        naviAgent.velocity = Vector3.one;

        isInNormal = false;
        animator = transform.parent.GetComponentInChildren<Animator>();
        currentState = AgentState.Idle;
        setStatus(agentBasic.initialStatus);
    }

    // Update is called once per frame
    void Update()
    {
        // 1. 状态机决策（agentStatus优先，决定agentState切换条件）
        if (currentState != AgentState.Dead && !isAttacking)
        {
            bool canAttack = false;
            if (detector.currentTarget)
            {
                float dist = (detector.currentTarget.transform.position - transform.position).magnitude;
                if (dist < agentBasic.range*1.01f && fireController.CanFire())
                {
                    canAttack = true;
                }
            }

            switch (agentBasic.agentStatus)
            {
                case AgentStatus.ASSIGN:
                    // 只要没到目标点就Move，不能Attack
                    if ((point - transform.parent.position).magnitude > 0.1f)
                    {
                        currentState = AgentState.Move;
                    }
                    else
                    {
                        currentState = AgentState.Idle;
                    }
                    break;
                case AgentStatus.PATROL:
                case AgentStatus.FOCUS:
                    // 侦测到敌人但不能攻击时只执行一次findpath
                    if (detector.currentTarget)
                    {
                        if (!canAttack)
                        {
                            if (!hasCalledFindPath)
                            {
                                findPath();
                                hasCalledFindPath = true;
                            }
                            currentState = AgentState.Move;
                        }
                        else
                        {
                            currentState = AgentState.Attack;
                            hasCalledFindPath = false;
                            isAttacking = true;
                        }
                    }
                    else
                    {
                        currentState = AgentState.Idle;
                    }
                    break;
                case AgentStatus.STAND_HOLD:
                    // 永远不会进入Move
                    if (canAttack)
                    {
                        currentState = AgentState.Attack;
                        isAttacking = true;
                    }
                    else
                    {
                        currentState = AgentState.Idle;
                    }
                    break;
                default:
                    // NORMAL状态，正常AI
                    if (canAttack)
                    {
                        currentState = AgentState.Attack;
                        isAttacking = true;
                    }
                    else if (detector.currentTarget)
                    {
                        currentState = AgentState.Move;
                    }
                    else
                    {
                        currentState = AgentState.Idle;
                    }
                    break;
            }
        }

        // 2. 动画机联动（agentState决定动画）
        switch (currentState)
        {
            case AgentState.Idle:
                animator.Play("Idle");
                break;
            case AgentState.Move:
                animator.Play("Move");
                if (!isAttacking)
                {
                    switch (agentBasic.agentStatus)
                    {
                        case AgentStatus.PATROL:
                        case AgentStatus.FOCUS:
                            if (detector.currentTarget)
                                SetNavDestination(point);
                            break;
                        case AgentStatus.FOLLOW:
                            SetNavDestination(player.transform.position);
                            break;
                        case AgentStatus.ASSIGN:
                            SetNavDestination(point);
                            break;
                        default:
                            break;
                    }
                    moveAgent();
                }
                break;
            case AgentState.Attack:
                animator.Play("Attack");
                if (detector.currentTarget)
                {
                    aim(detector.currentTarget.transform.position);
                    Vector3 tempVec = detector.currentTarget.transform.position - transform.position;
                    if (tempVec.magnitude < agentBasic.range*1.01f)
                    {
                        fireController.FireAuto(detector.currentTarget);
                    }
                }
                break;
            case AgentState.Dead:
                animator.Play("Dead");
                break;
        }

        //incomplete: when other techniques are triggered, reset timer and isInNormal by resetNormal()
        if (isInNormal && agentBasic.agentStatus == AgentStatus.NORMAL)
        {
            standHoldTimer += Time.deltaTime;
            if (standHoldTimer >= standHoldThreshold)
            {
                setStatus(AgentStatus.STAND_HOLD);
                resetNormal();
            }
        }
    }
    // PATROL状态下findpath调用标记
    private bool hasCalledFindPath = false;

    // PATROL状态下侦测到敌人但不能攻击时调用
    private void findPath()
    {
        if (detector == null || detector.currentTarget == null || detector.targetList == null || detector.targetList.Count <= 1)
            return;

        GameObject current = detector.currentTarget;
        Vector3 currentTargetPos = current.transform.position;
        if (agentBasic.agentStatus == AgentStatus.PATROL)
        {
            float preferDist = agentBasic.range * 0.6f;
            // 采样圆周上的若干点
            int sampleCount = 16;
            float maxSumDist = float.MinValue;
            Vector3 bestPoint = currentTargetPos;

            for (int i = 0; i < sampleCount; i++)
            {
                float angle = 2 * Mathf.PI * i / sampleCount;
                Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * preferDist;
                Vector3 candidate = currentTargetPos + offset;

                float sumDist = 0f;
                foreach (var obj in detector.targetList)
                {
                    if (obj == null || obj == current) continue;
                    sumDist += (candidate - obj.transform.position).magnitude;
                }
                // 若没有其它目标，直接用第一个点
                if (detector.targetList.Count <= 1)
                {
                    bestPoint = candidate;
                    break;
                }
                if (sumDist > maxSumDist)
                {
                    maxSumDist = sumDist;
                    bestPoint = candidate;
                }
            }
            point = bestPoint;
        }
        else if (agentBasic.agentStatus == AgentStatus.FOCUS)
        {
            // 以当前目标为圆心，半径0.2，采样圆周上若干点，选距离自己最近的点
            float focusDist = 0.2f;
            int sampleCount = 16;
            Vector3 selfPos = transform.parent != null ? transform.parent.position : transform.position;
            float minDist = float.MaxValue;
            Vector3 bestPoint = currentTargetPos;
            for (int i = 0; i < sampleCount; i++)
            {
                float angle = 2 * Mathf.PI * i / sampleCount;
                Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * focusDist;
                Vector3 candidate = currentTargetPos + offset;
                float distToSelf = (candidate - selfPos).sqrMagnitude;
                if (distToSelf < minDist)
                {
                    minDist = distToSelf;
                    bestPoint = candidate;
                }
            }
            point = bestPoint;
        }
        else return;

    }

    void resetNormal()
    {
        standHoldTimer = 0f;
        isInNormal = false;
    }

    //int timer = 0;
    void moveAgent()
    {
        if (naviAgent.enabled && naviAgent.remainingDistance > 0.02f)
        {
            naviAgent.isStopped = false;
            //Vector3 nextPos = naviAgent.nextPosition;
            Vector3 moveDir = naviAgent.desiredVelocity;

            //timer++;
            //if (timer % 100 == 0)
            //    Debug.Log("Move Direction: " + moveDir);

            if (Mathf.Abs(moveDir.normalized.x) > 0.1f)
            {
                // 朝向逻辑
                bool newIsLeft = moveDir.x < 0f;
                if (newIsLeft != isleft)
                {
                    isleft = newIsLeft;
                    transform.localScale = new Vector3(
                        Mathf.Abs(transform.localScale.x) * (isleft ? -1 : 1),
                        transform.localScale.y,
                        transform.localScale.z
                    );
                }
            }
            //move
            Vector3 temp = naviAgent.destination - transform.parent.position;
            if (temp.magnitude > 0.1f)
            {
                transform.parent.Translate(new Vector3(moveDir.x, moveDir.y, 0).normalized * agentBasic.moveSpeed * Time.deltaTime);
            }
            else
            {
                if (agentBasic.agentStatus == AgentStatus.ASSIGN)
                {
                    setStatus(AgentStatus.NORMAL);
                    isInNormal = true;
                }
            }

        }
        else
        {
            naviAgent.isStopped = true;
        }
    }

    // 通过NavMeshAgent设置目标点
    void SetNavDestination(Vector3 dest)
    {
        naviAgent.nextPosition = transform.position;
        if (naviAgent.enabled)
        {
            naviAgent.SetDestination(dest);
        }
    }

    // moveToPlayer和moveToPoint已由NavMeshAgent统一导航，无需实现

    void aim(Vector3 mousepos)
    {
        if (!isleft)
            weapontr.right = mousepos - weapontr.position;
        else weapontr.right = weapontr.position - mousepos;
    }

    public void setDetectRange(float curRange)
    {
        agentBasic.detectRange = curRange;
        detectCircle.radius = agentBasic.detectRange;
    }
    // When changing status, adjust attributes of different agents accordingly
    public void setStatus(AgentStatus status)
    {
        if (status != agentBasic.agentStatus)
        {
            //clear buff of former status

            //patrol
            if (agentBasic.agentStatus == AgentStatus.PATROL)
            {
                detector.transform.position = transform.position;
                detector.transform.parent = transform;
                if (agentBasic.agentType == AgentType.RANGER)
                {
                    calculator.addParam(CalculateItem.RANGE_FINALM, -0.1f);
                    calculator.addParam(CalculateItem.SPEED_FINALM, -0.2f);
                    agentBasic.setRange();
                    agentBasic.setMoveSpeed();
                    agentBasic.detectRange = agentBasic.range;
                }
                else
                {
                    agentBasic.detectRange = agentBasic.range;
                }
                setDetectRange(agentBasic.detectRange);
            }

            //focus
            if (agentBasic.agentStatus == AgentStatus.FOCUS)
            {
                detector.transform.position = transform.position;
                detector.transform.parent = transform;
                if (agentBasic.agentType == AgentType.ASSAULTER)
                {
                    calculator.addParam(CalculateItem.RANGE_FINALM, -0.1f);
                    calculator.addParam(CalculateItem.SPEED_FINALM, -0.2f);
                    agentBasic.setRange();
                    agentBasic.setMoveSpeed();
                    agentBasic.detectRange = agentBasic.range;
                }
                else
                {
                    agentBasic.detectRange = agentBasic.range;
                }
                setDetectRange(agentBasic.detectRange);
            }

            //standhold
            else if (agentBasic.agentStatus == AgentStatus.STAND_HOLD)
            {
                if (agentBasic.agentType == AgentType.SPINER)
                {
                    calculator.addParam(CalculateItem.RANGE_FINALM, -0.5f);
                    calculator.addParam(CalculateItem.ATTACK_FINALM, -0.2f);
                    agentBasic.setRange();
                    agentBasic.setDamage();
                }
                else
                {
                    calculator.addParam(CalculateItem.RANGE_FINALM, -0.3f);
                    agentBasic.setRange();
                }
                setDetectRange(agentBasic.range);
            }

            //follow, deprecated method
            else if (agentBasic.agentStatus == AgentStatus.FOLLOW)
            {
                if (agentBasic.agentType == AgentType.ASSAULTER)
                {
                    calculator.addParam(CalculateItem.SPEED_FINALM, -0.3f);
                    calculator.addParam(CalculateItem.HP_FINALM, -0.2f);
                    calculator.addParam(CalculateItem.ATTACK_FINALM, -0.1f);
                    agentBasic.setMoveSpeed();
                    agentBasic.setHP();
                    agentBasic.setDamage();
                }
                else if (agentBasic.agentType == AgentType.SCOUT)
                {
                    calculator.addParam(CalculateItem.SPEED_FINALM, -0.5f);
                    calculator.addParam(CalculateItem.RANGE_FINALM, -0.1f);
                    agentBasic.setMoveSpeed();
                    agentBasic.setRange();
                }
                setDetectRange(agentBasic.range);
            }

            //Assign
            else if (agentBasic.agentStatus == AgentStatus.ASSIGN)
            {
                if (agentBasic.agentType == AgentType.SCOUT)
                {
                    calculator.addParam(CalculateItem.SPEED_FINALM, -1.1f);
                    calculator.addParam(CalculateItem.RANGE_FINALM, 0.15f);
                    agentBasic.setMoveSpeed();
                    agentBasic.setRange();
                }
                setDetectRange(agentBasic.range);
            }

            agentBasic.agentStatus = status;

            //Patrol
            if (status == AgentStatus.PATROL)
            {
                if (agentBasic.agentType == AgentType.RANGER)
                {
                    //agentBasic.moveSpeed *= 1.2f;
                    //agentBasic.range *= 1.1f;
                    calculator.addParam(CalculateItem.RANGE_FINALM, 0.1f);
                    calculator.addParam(CalculateItem.SPEED_FINALM, 0.2f);
                    agentBasic.setRange();
                    agentBasic.setMoveSpeed();
                    agentBasic.detectRange = 2.5f * agentBasic.range;
                }
                else
                {
                    agentBasic.detectRange = 2 * agentBasic.range;
                }
                setDetectRange(agentBasic.detectRange);
                detector.transform.parent = null;
            }

            //Focus
            if (status == AgentStatus.FOCUS)
            {
                if (agentBasic.agentType == AgentType.ASSAULTER)
                {
                    //agentBasic.moveSpeed *= 1.2f;
                    //agentBasic.range *= 1.1f;
                    calculator.addParam(CalculateItem.RANGE_FINALM, 0.1f);
                    calculator.addParam(CalculateItem.SPEED_FINALM, 0.2f);
                    agentBasic.setRange();
                    agentBasic.setMoveSpeed();
                    agentBasic.detectRange = 2.5f * agentBasic.range;
                }
                else
                {
                    agentBasic.detectRange = 2 * agentBasic.range;
                }
                setDetectRange(agentBasic.detectRange);
                detector.transform.parent = null;
            }

            //standHold
            else if (status == AgentStatus.STAND_HOLD)
            {
                if (agentBasic.agentType == AgentType.SPINER)
                {
                    calculator.addParam(CalculateItem.RANGE_FINALM, 0.5f);
                    calculator.addParam(CalculateItem.ATTACK_FINALM, 0.2f);
                    agentBasic.setRange();
                    agentBasic.setDamage();
                }
                else
                {
                    calculator.addParam(CalculateItem.RANGE_FINALM, 0.3f);
                    agentBasic.setRange();
                }
                setDetectRange(agentBasic.range);
            }
            //follow
            else if (status == AgentStatus.FOLLOW)
            {
                if (agentBasic.agentType == AgentType.ASSAULTER)
                {
                    //agentBasic.moveSpeed *= 1.3f;
                    //agentBasic.maxHP *= 1.2f;
                    //agentBasic.damage *= 1.1f;
                    calculator.addParam(CalculateItem.SPEED_FINALM, 0.3f);
                    calculator.addParam(CalculateItem.HP_FINALM, 0.2f);
                    calculator.addParam(CalculateItem.ATTACK_FINALM, 0.1f);
                    agentBasic.setMoveSpeed();
                    agentBasic.setHP();
                    agentBasic.setDamage();
                }
                else if (agentBasic.agentType == AgentType.SCOUT)
                {
                    //agentBasic.moveSpeed *= 1.5f;
                    //agentBasic.range *= 1.1f;
                    calculator.addParam(CalculateItem.SPEED_FINALM, 0.5f);
                    calculator.addParam(CalculateItem.RANGE_FINALM, 0.1f);
                    agentBasic.setMoveSpeed();
                    agentBasic.setRange();
                }
                setDetectRange(agentBasic.range);
            }
            //assign
            else if (status == AgentStatus.ASSIGN)
            {
                if (agentBasic.agentType == AgentType.SCOUT)
                {
                    calculator.addParam(CalculateItem.SPEED_FINALM, 1.1f);
                    calculator.addParam(CalculateItem.RANGE_FINALM, -0.15f);
                    agentBasic.setMoveSpeed();
                    agentBasic.setRange();
                }
                setDetectRange(agentBasic.range);
            }
        }
        statusChangeEvent.Invoke();
    }

    public void onAgentClicked()
    {
        SelectionManager.instance.onItemClick(transform.parent.gameObject);
    }

    public void hurt(float damage, float mDamage, float armorPierce=0, float mArmorPierce=0)
    {
        float pFinal;
        float mFinal;
        if (damage <= 0f && mDamage <= 0f)//����,��������buff
        {
            pFinal = calculator.calculateFloat(damage, new CalculateItem[] { CalculateItem.HEAL_MULTIPLIER, CalculateItem.HEAL_FINALM });
            mFinal = calculator.calculateFloat(mDamage, new CalculateItem[] { CalculateItem.HEAL_MULTIPLIER, CalculateItem.HEAL_FINALM });
            currentHP -= pFinal + mFinal;
            //Debug.Log("cure - bullet");
            InfoCanvas.instance.addDamageNum(gameObject, -(pFinal+mFinal), DamageType.health);
        }
        else
        {
            float pFinalArmor = agentBasic.armor - armorPierce > 0 ? agentBasic.armor - armorPierce : 0;
            float mFinalArmor = agentBasic.mArmor - mArmorPierce > 0 ? agentBasic.mArmor - mArmorPierce : 0; 
            float pTempDamage = damage * (1 - (pFinalArmor / (pFinalArmor + 100)));
            float mTempDamage = mDamage * (1 - (mFinalArmor / (mFinalArmor + 100)));
            pFinal = calculator.calculateFloat(pTempDamage, new CalculateItem[] { CalculateItem.DAMAGE_REDUCTION });
            mFinal = calculator.calculateFloat(mTempDamage, new CalculateItem[] { CalculateItem.DAMAGE_REDUCTION });
            currentHP -= pFinal + mFinal;
            if (pFinal > 0)
            {
                InfoCanvas.instance.addDamageNum(gameObject, pFinal, DamageType.physics);
            }
            if (mFinal > 0)
            {
                InfoCanvas.instance.addDamageNum(gameObject, mFinal, DamageType.magic);
            }
        }
        if (currentHP <= 0 && currentState != AgentState.Dead)
        {
            // 禁用所有Collider2D，防止被检测和子弹交互
            foreach (var col in GetComponentsInChildren<Collider2D>())
            {
                col.enabled = false;
            }
            currentState = AgentState.Dead;
            // 死亡动画播放完毕后再回收对象，回收逻辑移到OnDeathAnimationEnd()
        }
        if (currentHP > agentBasic.maxHP) currentHP = agentBasic.maxHP;
        if(gameObject)
            InfoCanvas.instance.setHPBar(transform.parent.gameObject, currentHP / agentBasic.maxHP);
    }

    /// <summary>
    /// 死亡动画播放完毕后由动画事件调用，执行对象池回收等逻辑
    /// </summary>
    public override void OnDeathAnimationEnd()
    {
        Debug.Log("Death animation ended");
        int index = Array.IndexOf(InGameData.instance.agentList, transform.parent.gameObject);
        UIControllerInGame.instance.slotList[index].setSlotStatus(SlotStatus.AGENT_CD);
        Destroy(transform.parent.gameObject);
    }

    /// <summary>
    /// 攻击动画播放完毕后由动画事件调用，允许切换状态
    /// </summary>
    public override void OnAttackAnimationEnd()
    {
        //Debug.Log("Attack animation ended");
        isAttacking = false;
    }

    private void OnDisable()
    {
        InfoCanvas.instance.removeHPBar(transform.parent.gameObject);
        UIControllerInGame.instance.agentDestoyed(index);   
    }
}
