using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/WeaponData", order = 0)]
public class WeaponData : ScriptableObject
{
    public WeaponType weaponType;
    public float bulletSpeed;
    public List<float> damage;
    public List<float> mDamage;
    public int pierceNum;
    public List<float> fireInterval; //两次攻击之间的间隔，firecontroller会有一个计时器，计时器上限参考这个值
    public EntityType bulletType;
    public int magazing;
    public List<float> reloadTime;
    public TargetType targetType;
    public List<float> armorPierce;
    public List<float> mArmorPierce;

    public float getDamage(int level)
    {
        if(level > damage.Count)
        {
            return damage[damage.Count - 1];
        }
        return damage[level - 1];
    }
    public float getMDamage(int level)
    {
        if(level > mDamage.Count)
        {
            return mDamage[mDamage.Count - 1];
        }
        return mDamage[level - 1];
    }
    public float getFireInterval(int level)
    {
        if(level > fireInterval.Count)
        {
            return fireInterval[fireInterval.Count - 1];
        }
        return fireInterval[level - 1];
    }
    public float getReloadTime(int level)
    {
        if(level > reloadTime.Count)
        {
            return reloadTime[reloadTime.Count - 1];
        }
        return reloadTime[level - 1];
    }
    public float getArmorPierce(int level)
    {
        if(level > armorPierce.Count)
        {
            return armorPierce[armorPierce.Count - 1];
        }
        return armorPierce[level - 1];
    }
    public float getMArmorPierce(int level)
    {
        if(level > mArmorPierce.Count)
        {
            return mArmorPierce[mArmorPierce.Count - 1];
        }
        return mArmorPierce[level - 1];
    }
}