using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseBasic : WeaponBasic
{
    public FortificationData fortiData;
    protected BuffCalculator calculator;

    ///属性
    public float maxHP;
    public float range;
    public float detectRange;
    public float armor;
    public float mArmor;
    public EntityType entityType;

    protected WeaponBasic weaponBasic;

    [Header("事件组:每次set属性时,触发对应事件,监听该数据变化的")]
    public UnityEvent<float> eventSpeed;
    public UnityEvent<float> eventDamage;
    public UnityEvent<float> eventMagicDamage;
    public UnityEvent<float> eventRange;
    public UnityEvent<float> eventDetectRange;
    public UnityEvent<float> eventInterval;
    public UnityEvent<float> eventHp;
    public UnityEvent<float> eventMagazine;

    private void Awake()
    {
        calculator = GetComponent<BuffCalculator>();

        maxHP = fortiData.maxHP;
        range = fortiData.range;
        detectRange = fortiData.detectRange;
        armor = fortiData.armor;
        mArmor = fortiData.mArmor;
        entityType = fortiData.entityType;
    }

    ///set方法组
    ///set的逻辑是:原始数据+calculator计算
    ///重要的数据被set以后还会触发事件,重置所有的监听者
    public override void setDamage()
    {
        damage = calculator.calculateFloat(fortiData.damage, new CalculateItem[] { CalculateItem.ATTACK_MULTIPLIER, CalculateItem.ATTACK_ADDON, CalculateItem.ATTACK_FINALM });
        weaponBasic.damage = damage;
        eventDamage.Invoke(damage);
        Debug.Log("final damage:" + damage);
    }
    public override void setRange()
    {
        range = calculator.calculateFloat(fortiData.range, new CalculateItem[] { CalculateItem.RANGE_MULTIPLIER, CalculateItem.RANGE_FINALM });
        eventRange.Invoke(range);
    }
    public override void setFireInterval()
    {
        fireInterval = fortiData.fireInterval / (float)calculator.getItem(CalculateItem.ATTACK_SPEED_MULTIPLIER);
        weaponBasic.fireInterval = fireInterval;
        //event.Invoke()?
    }
    public override void setArmorPierce()
    {
        armorPierce = calculator.calculateFloat(fortiData.armorPierce, new CalculateItem[] { CalculateItem.ARMOR_PIERCE_MULTIPLIER, CalculateItem.ARMOR_PIERCE_ADDON });
        weaponBasic.armorPierce = armorPierce;
    }
    public override void setReloadTime()
    {
        reloadTime = calculator.calculateFloat(fortiData.reloadTime, new CalculateItem[] { CalculateItem.RELOAD_TIME_MULTIPLIER });
        weaponBasic.reloadTime = reloadTime;
    }
    public override void setHP()
    {
        maxHP = calculator.calculateFloat(fortiData.maxHP, new CalculateItem[] { CalculateItem.HP_MULTIPLIER, CalculateItem.HP_ADDON, CalculateItem.HP_FINALM });
        //event.Invoke()?
    }
}
