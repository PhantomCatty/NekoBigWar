using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//记录武器的基本信息，比如类型，攻击力等属性
//可以调用子弹脚本，来说明这个武器使用的是哪种子弹
//被firecontroller调用

public enum WeaponType
{
    Melee,
    SemiAuto,
    Burst,//连发，别搞得看不懂英文了
    Auto,
    Charge
}

public class WeaponBasic:MonoBehaviour
{
    protected WeaponData weaponData;

    public WeaponType weaponType;
    public float bulletSpeed;
    public float damage;
    public float mDamage;
    public int pierceNum;
    public float fireInterval;//两次攻击之间的间隔，firecontroller会有一个计时器，计时器上限参考这个值
    public EntityType bulletType;
    public int magazing;
    public float reloadTime;
    public TargetType targetType;
    public float armorPierce;//穿甲
    public float mArmorPierce;//无视抗性

    private void Start()
    {
        if (weaponData)
        {
            weaponType = weaponData.weaponType;
            bulletSpeed = weaponData.bulletSpeed;
            damage = weaponData.damage;
            mDamage = weaponData.mDamage;
            pierceNum = weaponData.pierceNum;
            fireInterval = weaponData.fireInterval;
            bulletType = weaponData.bulletType;
            magazing = weaponData.magazing;
            reloadTime = weaponData.reloadTime;
            targetType = weaponData.targetType;
            armorPierce = weaponData.armorPierce;
            mArmorPierce = weaponData.mArmorPierce;
        }
    }

    public void setWeapon(WeaponBasic temp)
    {
        weaponType = temp.weaponType;
        bulletSpeed = temp.bulletSpeed;
        damage = temp.damage;
        pierceNum = temp.pierceNum;
        fireInterval = temp.fireInterval;
        bulletType = temp.bulletType;
        magazing = temp.magazing;
        reloadTime = temp.reloadTime;
        targetType = temp.targetType;
        armorPierce = temp.armorPierce;
    }

    public virtual void setDamage(){}
    public virtual void setRange(){ }
    public virtual void setFireInterval(){}
    public virtual void setArmorPierce(){}
    public virtual void setMoveSpeed(){}
    public virtual void setReloadTime(){}
    public virtual void setHP(){}
}
