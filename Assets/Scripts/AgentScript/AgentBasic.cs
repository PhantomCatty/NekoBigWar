using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentBasic : WeaponBasic
{
    public AgentData agentData;
    private BuffCalculator calculator;

    ///����
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
    [Header("ͼƬ��Դ--��û�뵽�������������")]
    [Header("���������sobj�ı�Ҫ��")]
    public Sprite agentIcon;
    public Sprite skillIcon;
    public AgentStatus initialStatus;

    private WeaponBasic weaponBasic;

    [Header("�¼���:ÿ��set����ʱ,������Ӧ�¼�,���������ݱ仯��")]
    public UnityEvent<float> eventSpeed;
    public UnityEvent<float> eventDamage;
    public UnityEvent<float> eventRange;
    public UnityEvent<float> eventDetectRange;
    public UnityEvent<float> eventInterval;
    public UnityEvent<float> eventHp;
    public UnityEvent<float> eventMagazine;

    private void Awake()
    {
        calculator = GetComponent<BuffCalculator>();
        weaponType = agentData.weaponType;
        bulletSpeed = agentData.bulletSpeed;
        damage = agentData.damage;
        mDamage = agentData.mDamage;
        pierceNum = agentData.pierceNum;
        fireInterval = agentData.fireInterval;
        bulletType = agentData.bulletType;
        magazing = agentData.magazing;
        reloadTime = agentData.reloadTime;
        targetType = agentData.targetType;
        armorPierce = agentData.armorPierce;
        mArmorPierce = agentData.mArmorPierce;

        maxHP = agentData.maxHP;
        moveSpeed = agentData.moveSpeed;
        agentStatus = agentData.agentStatus;
        range = agentData.range;
        detectRange = agentData.detectRange;
        armor = agentData.armor;
        mArmor = agentData.mArmor;
        entityType = agentData.entityType;
        agentType = agentData.agentType;
        AssignCost = agentData.AssignCost;
        SkillCost = agentData.SkillCost;
        respawnTime = agentData.respawnTime;
        skillCD = agentData.skillCD;
        initialStatus = agentData.initialStatus;
        setWeapon();
    }

    //��controller�ṩ�Ľӿ�,�޸�����������,���,����������ֻ�ṩ��ָ�ӹ�.
    public void setWeapon()
    {
        weaponBasic = transform.GetChild(0).GetComponentInChildren<WeaponBasic>();
        weaponBasic.setWeapon(this);
    }

    ///set������
    ///set���߼���:ԭʼ����+calculator����
    ///��Ҫ�����ݱ�set�Ժ󻹻ᴥ���¼�,�������еļ�����
    public override void setDamage()
    {
        damage = calculator.calculateFloat(agentData.damage,new CalculateItem[] {CalculateItem.ATTACK_MULTIPLIER,CalculateItem.ATTACK_ADDON,CalculateItem.ATTACK_FINALM });
        mDamage = calculator.calculateFloat(agentData.mDamage, new CalculateItem[] { CalculateItem.MAGIC_ATTACK_MULTIPLIER, CalculateItem.MAGIC_ATTACK_ADDON, CalculateItem.MAGIC_ATTACK_FINALM });
        weaponBasic.damage = damage;
        weaponBasic.mDamage = mDamage;
        eventDamage.Invoke(damage);
    }
    public override void setRange()
    {
        range= calculator.calculateFloat(agentData.range, new CalculateItem[] { CalculateItem.RANGE_MULTIPLIER,CalculateItem.RANGE_FINALM });
        eventRange.Invoke(range);
    }
    public override void setFireInterval()
    {
        fireInterval = agentData.fireInterval / (float)calculator.getItem(CalculateItem.ATTACK_SPEED_MULTIPLIER);
        weaponBasic.fireInterval = fireInterval;
        //event.Invoke()?
    }
    public override void setArmorPierce()
    {
        armorPierce = calculator.calculateFloat(agentData.armorPierce, new CalculateItem[] { CalculateItem.ARMOR_PIERCE_MULTIPLIER, CalculateItem.ARMOR_PIERCE_ADDON });
        weaponBasic.armorPierce = armorPierce;
    }
    public override void setMoveSpeed()
    {
        moveSpeed = calculator.calculateFloat(agentData.moveSpeed, new CalculateItem[] { CalculateItem.SPEED_MULTIPLIER, CalculateItem.SPEED_ADDON, CalculateItem.SPEED_FINALM });
        if (moveSpeed < 0) moveSpeed = 0;
        eventSpeed.Invoke(moveSpeed);
    }
    public override void setReloadTime()
    {
        reloadTime = calculator.calculateFloat(agentData.reloadTime, new CalculateItem[] { CalculateItem.RELOAD_TIME_MULTIPLIER });
        weaponBasic.reloadTime = reloadTime;
    }
    public override void setHP()
    {
        maxHP = calculator.calculateFloat(agentData.maxHP, new CalculateItem[] { CalculateItem.HP_MULTIPLIER, CalculateItem.HP_ADDON, CalculateItem.HP_FINALM });
        //event.Invoke()?
    }
}

public enum AgentStatus
{
    STAND_HOLD = 0,
    PATROL = 1,
    FOLLOW = 2,
    ASSIGN = 3,
    NORMAL = 4,
    FOCUS = 5
}

public enum AgentType
{
    //support
    MEDIC = 0,        //ҽʦ,�̶���,��������,һ��û��ս���ӳ�
    ENGINEER = 1,     //����ʦ,��������,һ��û��ս���ӳ�
    //entourage
    ASSAULTER = 2,  //������,���ܾ���,������ɫ,�Է������ع�(����)
    SCOUT = 3,        //���,���ٿ�,�����и�������,�ʺϴ��λ�
    //ranger
    RANGER = 4,       //�����,��Χ��,���ٸ�,֧Ԯ�;�������ǿ���ǹ�������Բ�
    HUNTER = 5,       //����,�๦�ܵ����������,������������
    //watcher
    SPINER = 7,       //�ѻ���,����Χ�������,��������,վ׮����,��Ҫ����
    GUARD = 8         //����,��Ϊ��ս,��ΧС,�˺���̹�ȼ��,���ܻᳰ������

        //��Ȼһ��ʼ�����������������С��Χ���仹��վ׮,�������ڿ���,ְҵ����������ĳ��ս��,ֻ�������ֶȺ�ս����ֵ����
}