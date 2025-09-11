using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentData", menuName = "ScriptableObject/AgentData", order = 0)]
public class AgentData : WeaponData
{
    public string agentName;
    public int maxLevel;
    public List<float> maxHP;
    public List<float> moveSpeed;
    public AgentStatus agentStatus;
    public List<float> range;
    public List<float> detectRange;
    public List<float> armor;
    public List<float> mArmor;
    public List<int> upgradeCost;
    public AgentType agentType;
    public EntityType entityType;
    public float AssignCost;
    public float SkillCost;
    public List<float> respawnTime;
    public List<float> skillCD;
    public AgentStatus initialStatus;

    [Header("ͼƬ��Դ--��û�뵽�������������")]
    [Header("���������sobj�ı�Ҫ��")]
    public Sprite agentIcon;
    public Sprite skillIcon;

    public float getMaxHP(int level)
    {
        if(level > maxHP.Count)
        {
            return maxHP[maxHP.Count - 1];
        }
        return maxHP[level - 1];
    }
    public float getMoveSpeed(int level)
    {
        if(level > moveSpeed.Count)
        {
            return moveSpeed[moveSpeed.Count - 1];
        }
        return moveSpeed[level - 1];
    }
    public float getRange(int level)
    {
        if(level > range.Count)
        {
            return range[range.Count - 1];
        }
        return range[level - 1];
    }
    public float getDetectRange(int level)
    {
        if(level > detectRange.Count)
        {
            return detectRange[detectRange.Count - 1];
        }
        return detectRange[level - 1];
    }
    public float getArmor(int level)
    {
        if(level > armor.Count)
        {
            return armor[armor.Count - 1];
        }
        return armor[level - 1];
    }
    public float getMArmor(int level)
    {
        if(level > mArmor.Count)
        {
            return mArmor[mArmor.Count - 1];
        }
        return mArmor[level - 1];
    }
    public float getRespawnTime(int level)
    {
        if(level > respawnTime.Count)
        {
            return respawnTime[respawnTime.Count - 1];
        }
        return respawnTime[level - 1];
    }
    public float getSkillCD(int level)
    {
        if(level > skillCD.Count)
        {
            return skillCD[skillCD.Count - 1];
        }
        return skillCD[level - 1];
    }
}