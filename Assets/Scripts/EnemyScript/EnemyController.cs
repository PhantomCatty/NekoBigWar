using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Dead
}

public class EnemyController : BaseAnimEventController
{
    // Reference to the basic enemy stats and settings
    [SerializeField]
    public EnemyBasic enemyBasic;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public GameObject basement;
    public BuffCalculator calculator;
    private Vector3 moveDirection;
    private Vector3 aimDirection;
    private NavMeshAgent naviAgent;

    public float currentHP;
    public int currentAmmo;

    public bool isleft;

    Transform weapontr;
    FireController fireController;
    EnemyAimDetector detector;
    CircleCollider2D detectCircle;

    //Enemy state and animator
    public EnemyState currentState;
    private Animator animator;
    private bool isAttacking = false; // 攻击动画播放中


    // Start is called before the first frame update
    void Awake()
    {
        basement = GameObject.Find("Base");
        player = InGameData.instance.player;
        enemyBasic = GetComponent<EnemyBasic>();
        calculator = GetComponent<BuffCalculator>();
    }

    private void Start()
    {
        enemyBasic = GetComponent<EnemyBasic>();
        detectCircle = GetComponentInChildren<CircleCollider2D>();
        detectCircle.radius = enemyBasic.detectRange;
        detector = GetComponentInChildren<EnemyAimDetector>();
        fireController = GetComponentInChildren<FireController>();
        weapontr = transform.Find("WeaponPlace").transform;
        fireController.calculator = calculator;
        detector.followPrefer1 = enemyBasic.followPrefer1;
        detector.targetType = enemyBasic.targetType;
        detector.followPreferDefault = enemyBasic.followPreferDefault;
        naviAgent = GetComponent<NavMeshAgent>();
        naviAgent.updateRotation = false;
        naviAgent.updatePosition = false;
        naviAgent.updateUpAxis = false;
        naviAgent.velocity = Vector3.one;
        animator = transform.parent.GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        currentHP = enemyBasic.maxHP;
        calculator.clearBuff();
        currentState = EnemyState.Idle;
    }

    private void OnDisable()
    {
        if (gameObject)
        {
            InfoCanvas.instance.removeHPBar(transform.parent.gameObject);
            calculator.clearBuff();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 状态切换逻辑
        if (currentState != EnemyState.Dead && !isAttacking)
        {
            bool canAttack = false;
            if (detector.currentTarget)
            {
                float dist = (detector.currentTarget.transform.position - weapontr.position).magnitude;
                // 假设fireController有冷却判断方法CanFire()
                if (dist < enemyBasic.range*1.01f && fireController.CanFire())
                //if (fireController.CanFire())
                {
                    canAttack = true;
                }
            }
            if (canAttack)
            {
                isAttacking = true; // 攻击动画开始
                currentState = EnemyState.Attack;
            }
            else if (detector.followTarget)
            {
                currentState = EnemyState.Move;
            }
            else
            {
                currentState = EnemyState.Idle;
            }
        }
        //Debug.Log("Current State: " + currentState + ", isAttacking: " + isAttacking);
        // 行为执行
        switch (currentState)
        {
            case EnemyState.Idle:
                // Handle idle state behavior
                animator.Play("Idle");
                break;
            case EnemyState.Move:
                // Handle move state behavior
                animator.Play("Move");
                findDirection();
                enemyMove();
                break;
            case EnemyState.Attack:
                // Handle attack state behavior
                animator.Play("Attack");
                if (detector.currentTarget)
                {
                    aim(detector.currentTarget.transform.position);
                    if (aimDirection.magnitude < enemyBasic.range*1.01f)
                    {
                        fireController.FireAuto();
                    }
                }
                break;
            case EnemyState.Dead:
                // Handle dead state behavior
                animator.Play("Dead");
                break;
        }


    }

    void aim(Vector3 mousepos)
    {
        if (!isleft)
            weapontr.right = aimDirection = mousepos - weapontr.position;
        else weapontr.right = aimDirection = weapontr.position - mousepos;
    }

    private void findDirection()
    {
        naviAgent.nextPosition = transform.position;
        if (detector.followTarget)
        {
            //moveDirection = detector.followTarget.transform.position - transform.parent.position;
            if (naviAgent.enabled)
            {
                naviAgent.SetDestination(detector.followTarget.transform.position);
            }
            moveDirection = naviAgent.desiredVelocity;
        }
    }

    private void enemyMove()
    {
        if (Mathf.Abs(moveDirection.normalized.x) > 0.1f)
        {
            bool newIsLeft = moveDirection.x < 0f;
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

        //timer++;
        //if (timer % 20 == 0)
        //    Debug.Log("Move Direction: " + moveDirection);

        if ((detector.followTarget.transform.position - transform.parent.position).magnitude > enemyBasic.range * 0.8f)
        {
            transform.parent.Translate(moveDirection.normalized * enemyBasic.moveSpeed * Time.deltaTime);
        }
    }

    public void hurt(float damage, float mDamage, float armorPierce = 0, float mArmorPierce = 0)
    {
        float pFinal, mFinal;
        if (damage <= 0f && mDamage <= 0f)//����
        {
            //�����������ƺͷ������ƣ�����ʱ�����㻤�׺Ϳ���
            pFinal = calculator.calculateFloat(damage, new CalculateItem[] { CalculateItem.HEAL_MULTIPLIER, CalculateItem.HEAL_FINALM });
            mFinal = calculator.calculateFloat(mDamage, new CalculateItem[] { CalculateItem.HEAL_MULTIPLIER, CalculateItem.HEAL_FINALM });
            currentHP -= pFinal + mFinal;
            InfoCanvas.instance.addDamageNum(gameObject, -(pFinal + mFinal), DamageType.health);
        }
        else
        {
            float pFinalArmor = enemyBasic.armor - armorPierce > 0 ? enemyBasic.armor - armorPierce : 0;
            float mFinalArmor = enemyBasic.mArmor - mArmorPierce > 0 ? enemyBasic.mArmor - mArmorPierce : 0;
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
        if (currentHP <= 0 && currentState != EnemyState.Dead)
        {
            currentState = EnemyState.Dead;
            // 禁用所有Collider2D，防止被检测和子弹交互
            foreach (var col in GetComponentsInChildren<Collider2D>())
            {
                col.enabled = false;
            }
            // 死亡动画播放完毕后再回收对象
            // 回收逻辑移到OnDeathAnimationEnd()

            //trigger cost and exp addon
            InGameData.instance.addExperience(enemyBasic.rewardExperience);
            InGameData.instance.addCost(enemyBasic.rewardCost);
        }
        if (currentHP > enemyBasic.maxHP) currentHP = enemyBasic.maxHP;
        if (gameObject)
            InfoCanvas.instance.setHPBar(transform.parent.gameObject, currentHP / enemyBasic.maxHP);
    }

    /// <summary>
    /// 死亡动画播放完毕后由动画事件调用，执行对象池回收等逻辑
    /// </summary>
    override public void OnDeathAnimationEnd()
    {
        ObjectPool.instance.pushObject(transform.parent.gameObject, enemyBasic.entityType);
        EnemySpawnController.curAmount--;
        InGameData.instance.addKills();
        
    }

    /// <summary>
    /// 攻击动画播放完毕后由动画事件调用，允许切换状态
    /// </summary>
    override public void OnAttackAnimationEnd()
    {
        isAttacking = false;
    }
    
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.tag == "Bullet")
    //    {
    //        this.enemyBasic.currentHP-= other.GetComponent<BulletBasic>().damage;
    //        if (enemyBasic.currentHP <= 0)
    //        {
    //            ObjectPool.instance.pushObject(this.gameObject, EntityType.EN_PATROLLER);
    //            EnemySpawnController.curAmount--;
    //        }
    //    }
    //}
}
