using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FortificationData", menuName = "ScriptableObject/FortificationData", order = 0)]
public class FortificationData : WeaponData
{
    public float maxHP;
    public float range;
    public float detectRange;
    public float armor;
    public float mArmor;
    public EntityType entityType;
}
