using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillBasic : MonoBehaviour
{
    [MultiEnum] public SkillExertType exertType;
    [MultiEnum] public SkillEffectType effectType;
    [MultiEnum] public SkillType skillType;
    public Vector3 position;
    public List<GameObject> targetList;
    public float areaRadius = 0f;//���ܵķ�Χ,Ĭ��Ϊ0
    public float timer = 0f;//��ʱ��,�����÷�����ʵ��
    public int counter;//������,�÷�����ʵ���������
    public SkillState skillState;
    protected AgentBasic agentBasic;
    protected FortificationBasic fortificationBasic;
    protected EnemyBasic enemyBasic;
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
        else if (skillType == SkillType.SUMMON)
        {
            startSummon();
        }
    }

    //��ֹ��ʱ����ʲô��Ķ���,���ܻ�����
    public virtual void pauseSkill()
    {

    }

    //ֹͣ&��λ,������������ü��ܵ�״̬,���ܻ�����
    //public void endSkill()
    //{
    //
    //}

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
