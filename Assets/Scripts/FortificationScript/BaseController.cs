using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

/// <summary>
/// 一语双关的类,既表示Base-Controller(基地控制器),也表示Basic-Controller(控制器类的基类)
/// 只记录基本逻辑,不包含战斗逻辑,包含一个destory方法用作关卡控制
/// </summary>
public class BaseController : MonoBehaviour
{
    public BaseBasic baseBasic;

    public float currentHP;
    [HideInInspector]
    public GameObject player;
    public BuffCalculator calculator;

    // Start is called before the first frame update
    void Start()
    {
        calculator = GetComponent<BuffCalculator>();
        baseBasic = GetComponent<BaseBasic>();
        currentHP = baseBasic.maxHP;
        InfoCanvas.instance.addHPBar(transform.parent.gameObject, TargetType.BASE);
        InGameData.instance.allyBaseList.Add(transform.parent.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public virtual void hurt(float damage, float mDamage, float armorPierce=0, float mArmorPierce=0)
    {
        float pFinal;
        float mFinal;
        if (damage < 0f)//治疗,套用治疗buff
        {
            pFinal = calculator.calculateFloat(damage, new CalculateItem[] { CalculateItem.HEAL_MULTIPLIER, CalculateItem.HEAL_FINALM });
            mFinal = calculator.calculateFloat(damage, new CalculateItem[] { CalculateItem.HEAL_MULTIPLIER, CalculateItem.HEAL_FINALM });
            currentHP -= pFinal + mFinal;
            InfoCanvas.instance.addDamageNum(gameObject, pFinal+mFinal, DamageType.health); 
        }
        else
        {
            float pFinalArmor = baseBasic.armor - armorPierce > 0 ? baseBasic.armor - armorPierce : 0;
            float mFinalArmor = baseBasic.mArmor - mArmorPierce > 0 ? baseBasic.mArmor - mArmorPierce : 0;
            float pTempDamage = damage * (1 - (pFinalArmor / (pFinalArmor + 100)));
            float mTempDamage = mDamage * (1 - (mFinalArmor / (mFinalArmor + 100)));
            pFinal = calculator.calculateFloat(pTempDamage, new CalculateItem[] { CalculateItem.DAMAGE_REDUCTION });
            mFinal = calculator.calculateFloat(mTempDamage, new CalculateItem[] { CalculateItem.DAMAGE_REDUCTION });
            currentHP -= pFinal + mFinal;
            if(pFinal > 0){
                InfoCanvas.instance.addDamageNum(gameObject, pFinal, DamageType.physics); 
            }
            if(mFinal > 0){
                InfoCanvas.instance.addDamageNum(gameObject, mFinal, DamageType.magic);
            }
        }
        if (currentHP <= 0)
        {
            InGameData.instance.winCondition.minusCondition(GameCondition.ALLY_BASE, 1f);
            Destroy(transform.parent.gameObject);
        }
        if (currentHP > baseBasic.maxHP) currentHP = baseBasic.maxHP;
        if (gameObject)
            InfoCanvas.instance.setHPBar(transform.parent.gameObject, currentHP / baseBasic.maxHP);
    }

    void OnDestroy()
    {
        InGameData.instance.winCondition.minusCondition(GameCondition.ALLY_BASE, 1);
    }
}
