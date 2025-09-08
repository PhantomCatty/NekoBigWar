using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class FortificationController : BaseController
{
    [HideInInspector]
    public int index;
    [SerializeField]
    public FortificationBasic fortiBasic;
    public int currentAmmo;

    public float maxVelocity;
    public float currVelocity;
    public float acceleration;

    public bool isleft;
    Transform weapontr;
    [HideInInspector]
    public FireController fireController;
    public AgentAimDetector detector;
    CircleCollider2D detectCircle;

    public Vector3 point;

    // Start is called before the first frame update
    void Start()
    {
        calculator = GetComponent<BuffCalculator>();
        fortiBasic = GetComponent<FortificationBasic>();
        currentHP = fortiBasic.maxHP;
        detectCircle = GetComponentInChildren<CircleCollider2D>();
        detectCircle.radius = fortiBasic.detectRange;
        detector = GetComponentInChildren<AgentAimDetector>();
        fireController = GetComponentInChildren<FireController>();
        fireController.calculator = calculator;
        weapontr = transform.Find("WeaponPlace").transform;
        player = InGameData.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (detector.currentTarget)
        {
            aim(detector.currentTarget.transform.position);
            Vector3 tempVec = detector.currentTarget.transform.position - transform.position;
            if (tempVec.magnitude < fortiBasic.range)
            {
                fireController.FireAuto(detector.currentTarget);
            }

        }
    }

    void aim(Vector3 mousepos)
    {
        if (!isleft)
            weapontr.right = mousepos - weapontr.position;
        else weapontr.right = weapontr.position - mousepos;
    }

    public override void hurt(float damage, float mDamage, float armorPierce=0, float mArmorPierce=0)
    {
        float pFinal, mFinal;
        if (damage <= 0f && mDamage <= 0f)//治疗
        {
            //计算物理治疗和法术治疗，治疗时不计算护甲和抗性
            pFinal = calculator.calculateFloat(damage, new CalculateItem[] { CalculateItem.HEAL_MULTIPLIER, CalculateItem.HEAL_FINALM });
            mFinal = calculator.calculateFloat(mDamage, new CalculateItem[] { CalculateItem.HEAL_MULTIPLIER, CalculateItem.HEAL_FINALM });
            currentHP -= pFinal + mFinal;
            InfoCanvas.instance.addDamageNum(gameObject, -(pFinal+mFinal), DamageType.health); 
        }
        else
        {
            float pFinalArmor = fortiBasic.armor - armorPierce > 0 ? fortiBasic.armor - armorPierce : 0;
            float mFinalArmor = fortiBasic.mArmor - mArmorPierce > 0 ? fortiBasic.mArmor - mArmorPierce : 0;
            float pTempDamage = damage * (1 - (pFinalArmor / (pFinalArmor + 100)));
            float mTempDamage = mDamage * (1 - (mFinalArmor / (mFinalArmor + 100)));
            pFinal = calculator.calculateFloat(pTempDamage, new CalculateItem[] { CalculateItem.DAMAGE_REDUCTION });
            mFinal = calculator.calculateFloat(mTempDamage, new CalculateItem[] { CalculateItem.DAMAGE_REDUCTION });
            currentHP -= pFinal + mFinal;
            if(pFinal > 0){
                InfoCanvas.instance.addDamageNum(gameObject, pFinal, DamageType.physics); 
            }
            if(mFinal > 0){
                InfoCanvas.instance.addDamageNum(gameObject, mFinal, DamageType.magic);
            }
        }

        if (currentHP <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
        if (currentHP > fortiBasic.maxHP) currentHP = fortiBasic.maxHP;
        if (gameObject)
            InfoCanvas.instance.setHPBar(transform.parent.gameObject, currentHP / fortiBasic.maxHP);
    }
}
