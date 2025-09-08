using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/WeaponData", order = 0)]
public class WeaponData : ScriptableObject
{
    public WeaponType weaponType;
    public float bulletSpeed;
    public float damage;
    public float mDamage;
    public int pierceNum;
    public float fireInterval; //两次攻击之间的间隔，firecontroller会有一个计时器，计时器上限参考这个值
    public EntityType bulletType;
    public int magazing;
    public float reloadTime;
    public TargetType targetType;
    public float armorPierce;
    public float mArmorPierce;
}