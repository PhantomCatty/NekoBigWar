using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentData", menuName = "ScriptableObject/AgentData", order = 0)]
public class AgentData : WeaponData
{
    public float maxHP;
    public float moveSpeed;
    public AgentStatus agentStatus;
    public float range;
    public float detectRange;
    public float armor;
    public float mArmor;
    public AgentType agentType;
    public EntityType entityType;
    public float AssignCost;
    public float SkillCost;
    public float respawnTime;
    public float skillCD;
    public AgentStatus initialStatus;

    [Header("ͼƬ��Դ--��û�뵽�������������")]
    [Header("���������sobj�ı�Ҫ��")]
    public Sprite agentIcon;
    public Sprite skillIcon;
}