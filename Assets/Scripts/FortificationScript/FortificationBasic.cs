using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FortificationBasic : BaseBasic
{
     
    private void Awake()
    {
        calculator = GetComponent<BuffCalculator>();
        weaponType = fortiData.weaponType;
        bulletSpeed = fortiData.bulletSpeed;
        damage = fortiData.damage;
        mDamage = fortiData.mDamage;
        pierceNum = fortiData.pierceNum;
        fireInterval = fortiData.fireInterval;
        bulletType = fortiData.bulletType;
        magazing = fortiData.magazing;
        reloadTime = fortiData.reloadTime;
        targetType = fortiData.targetType;

        setWeapon();
    }

    public void setWeapon()
    {
        weaponBasic = transform.GetChild(0).GetComponentInChildren<WeaponBasic>();
        weaponBasic.setWeapon(this);
    }

    ///set方法组
    ///set的逻辑是:原始数据+calculator计算
    ///重要的数据被set以后还会触发事件,重置所有的监听者
    public override void setDamage()
    {
        damage = calculator.calculateFloat(fortiData.damage, new CalculateItem[] { CalculateItem.ATTACK_MULTIPLIER, CalculateItem.ATTACK_ADDON, CalculateItem.ATTACK_FINALM });
        mDamage = calculator.calculateFloat(fortiData.mDamage, new CalculateItem[] { CalculateItem.MAGIC_ATTACK_MULTIPLIER, CalculateItem.MAGIC_ATTACK_ADDON, CalculateItem.MAGIC_ATTACK_FINALM });
        weaponBasic.damage = damage;
        weaponBasic.mDamage = mDamage;
        eventDamage.Invoke(damage);
        eventMagicDamage.Invoke(mDamage);
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
