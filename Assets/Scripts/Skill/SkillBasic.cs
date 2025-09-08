using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBasic : MonoBehaviour
{
    [MultiEnum] public SkillExertType exertType;
    [MultiEnum] public SkillEffectType effectType;
    [MultiEnum] public SkillType skillType;
    public Vector3 position;
    public List<GameObject> targetList;
    public float skillCost;
    public float skillCD;
    public float areaRadius=0f;//技能的范围,默认为0
    public float timer=0f;//计时器,超过时间就停止技能效果
    public int counter;//计数器,根据实际情况(也就是虚函数具体对应的那个函数),计数为0时停止效果
    public SkillState skillState;
    protected AgentBasic agentBasic;
    public bool isArea;

    public void startSkill()
    {
        agentBasic = GetComponent<AgentBasic>();
        skillState = SkillState.RELEASING;
        if (skillType == SkillType.BUFF_DEBUFF)
        {
            startAddBuff();
        }
        else if (skillType == SkillType.CREATE_EFFECT)
        {
            startEffect();
        }
        else if(skillType == SkillType.SUMMON)
        {
            startSummon();
        }
    }

    public void endSkill()
    {

    }


    public virtual void startAddBuff()
    {
        
    }

    public virtual void endAddBuff()
    {

    }

    public virtual void startEffect()
    {

    }

    public virtual void endEffect()
    {

    }

    public virtual void startSummon()
    {

    }

    public virtual void endSummon()
    {

    }
}

/// <summary>
/// 技能施放类型,决定技能被施放到什么对象上(注意,不是说技能对谁起作用)
/// 
/// </summary>
[System.Flags]
public enum SkillExertType
{
    SELF=1,
    ALLY=2,
    ENEMY=4,
    ALLY_FORTIFICATION=8,
    ENEMY_FORTIFICATION=16,
    POSITION=32,
}

/// <summary>
///技能作用类型,决定了技能对哪些对象起作用,或者说,可以和谁交互 
///并不是一直用得上,SELF+BUFF就用不上这个
/// </summary>
[System.Flags]
public enum SkillEffectType
{
    ALLY=1,
    ENEMY=2,
    ALLY_FORTIFICATION=4,
    ENEMY_FORTIFICATION=8
}

public enum SkillType
{
    BUFF_DEBUFF=0,//buff/debuff,纯加buff的
    CREATE_EFFECT=1,//创造一个物体,传统,经典的"施放技能",放出一个特效物
    SUMMON=2,//召唤一个单位,和上面的区别是,召唤物自己有类,自己管自己,特效物就是个效果,归自己管
}

/// <summary>
/// 技能状态,描述技能处于生命周期的哪个阶段
/// </summary>
public enum SkillState
{
    RELEASING=0,//释放中,也就是生效前
    ACTIVE=1,//生效中
    ENDING=2,//结束中,也就是生效后,技能不再有效的状态
    PAUSED=3,//暂停中,被动技能可能会用得到
}