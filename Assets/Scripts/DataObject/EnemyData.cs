using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObject/EnemyData", order = 0)]
public class EnemyData : WeaponData
{
    public float maxHP;
    public float moveSpeed;
    public float range;
    public float detectRange;
    public float armor;
    public float mArmor;
    public TargetType followPrefer1;
    public EntityType entityType;
    public TargetType followPreferDefault;
    public float RewardExperience;
    public float RewardCost;
}
