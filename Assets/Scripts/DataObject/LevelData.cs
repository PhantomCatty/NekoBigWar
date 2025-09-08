using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObject/LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    public int maxLevel;
    public List<float> LevelExperience;
    public List<float> LevelRewardCoin;
    public List<LevelItemPool> levelItemPools;
    public List<int> ItemPoolLevelRequirement;
}

[Serializable]
public struct LevelItemPool
{
    public List<ItemBasic> items;
    public List<float> weights;
}