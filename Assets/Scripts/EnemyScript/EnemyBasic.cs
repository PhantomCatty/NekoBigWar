using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBasic : WeaponBasic
{
    public EnemyData enemyData;
    private BuffCalculator calculator;

    ///����
    public float maxHP;
    public float moveSpeed;
    public float range;
    public float detectRange;
    public float armor;
    public float mArmor;
    public float rewardCost;
    public float rewardExperience;
    public TargetType followPrefer1;
    public EntityType entityType;
    public TargetType followPreferDefault;
    private WeaponBasic weaponBasic;

    [Header("�¼���:ÿ��set����ʱ,������Ӧ�¼�,���������ݱ仯��")]
    public UnityEvent<float> eventSpeed;
    public UnityEvent<float> eventDamage;
    public UnityEvent<float> eventRange;
    public UnityEvent<float> eventDetectRange;
    public UnityEvent<float> eventInterval;
    public UnityEvent<float> eventHp;
    public UnityEvent<float> eventMagazine;

    private void Awake()
    {
        calculator = GetComponent<BuffCalculator>();
        weaponType = enemyData.weaponType;
        bulletSpeed = enemyData.bulletSpeed;
        damage = enemyData.damage[0];
        mDamage = enemyData.mDamage[0];
        pierceNum = enemyData.pierceNum;
        fireInterval = enemyData.fireInterval[0];
        bulletType = enemyData.bulletType;
        magazing = enemyData.magazing;
        reloadTime = enemyData.reloadTime[0];
        targetType = enemyData.targetType;
        armorPierce = enemyData.armorPierce[0];
        mArmorPierce = enemyData.mArmorPierce[0];
        followPrefer1 = enemyData.followPrefer1;
        followPreferDefault = enemyData.followPreferDefault;

        maxHP = enemyData.maxHP;
        moveSpeed = enemyData.moveSpeed;
        range = enemyData.range;
        detectRange = enemyData.detectRange;
        armor = enemyData.armor;
        mArmor = enemyData.mArmor;
        entityType = enemyData.entityType;

        rewardCost = enemyData.RewardCost;
        rewardExperience = enemyData.RewardExperience;
        setWeapon();
    }

    public void setWeapon()
    {
        weaponBasic = transform.GetChild(0).GetComponentInChildren<WeaponBasic>();
        weaponBasic.setWeapon(this);
    }

    ///set������
    ///set���߼���:ԭʼ����+calculator����
    ///��Ҫ�����ݱ�set�Ժ󻹻ᴥ���¼�,�������еļ�����
    public override void setDamage()
    {
        damage = calculator.calculateFloat(enemyData.damage[0], new CalculateItem[] { CalculateItem.ATTACK_MULTIPLIER, CalculateItem.ATTACK_ADDON, CalculateItem.ATTACK_FINALM });
        mDamage = calculator.calculateFloat(enemyData.mDamage[0], new CalculateItem[] { CalculateItem.MAGIC_ATTACK_MULTIPLIER, CalculateItem.MAGIC_ATTACK_ADDON, CalculateItem.MAGIC_ATTACK_FINALM });
        weaponBasic.damage = damage;
        weaponBasic.mDamage = mDamage;
        eventDamage.Invoke(damage);
    }
    public override void setRange()
    {
        range = calculator.calculateFloat(enemyData.range, new CalculateItem[] { CalculateItem.RANGE_MULTIPLIER, CalculateItem.RANGE_FINALM });
        eventRange.Invoke(range);
    }
    public override void setFireInterval()
    {
        fireInterval = enemyData.fireInterval[0] / (float)calculator.getItem(CalculateItem.ATTACK_SPEED_MULTIPLIER);
        weaponBasic.fireInterval = fireInterval;
        //event.Invoke()?
    }
    public override void setArmorPierce()
    {
        armorPierce = calculator.calculateFloat(enemyData.armorPierce[0], new CalculateItem[] { CalculateItem.ARMOR_PIERCE_MULTIPLIER, CalculateItem.ARMOR_PIERCE_ADDON });
        weaponBasic.armorPierce = armorPierce;
    }
    public override void setMoveSpeed()
    {
        moveSpeed = calculator.calculateFloat(enemyData.moveSpeed, new CalculateItem[] { CalculateItem.SPEED_MULTIPLIER, CalculateItem.SPEED_ADDON, CalculateItem.SPEED_FINALM });
        if (moveSpeed < 0) moveSpeed = 0;
        eventSpeed.Invoke(moveSpeed);
    }
    public override void setReloadTime()
    {
        reloadTime = calculator.calculateFloat(enemyData.reloadTime[0], new CalculateItem[] { CalculateItem.RELOAD_TIME_MULTIPLIER });
        weaponBasic.reloadTime = reloadTime;
    }
    public override void setHP()
    {
        maxHP = calculator.calculateFloat(enemyData.maxHP, new CalculateItem[] { CalculateItem.HP_MULTIPLIER, CalculateItem.HP_ADDON, CalculateItem.HP_FINALM });
        //event.Invoke()?
    }
}
