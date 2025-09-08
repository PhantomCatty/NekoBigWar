using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASkillFletcher : SkillBasic
{
    BuffCalculator calculator;
    FireController fireC;
    public override void startAddBuff()
    {
        timer = 6f;
        counter = 10000;
        skillState = SkillState.ACTIVE;
        calculator.addParam(CalculateItem.ATTACK_SPEED_MULTIPLIER, 0.5f);
        calculator.addParam(CalculateItem.RELOAD_TIME_MULTIPLIER, -0.999f);
        //下面这行用事件比较好,不过,技能是最高权限吧,所以随便吧哈哈
        agentBasic.setFireInterval();
        agentBasic.setReloadTime();
        //calculator.addParam(CalculateItem.FREE_AMMO, 0.1f);
    }

    public override void endAddBuff()
    {
        calculator.addParam(CalculateItem.ATTACK_SPEED_MULTIPLIER, -0.5f);
        //calculator.addParam(CalculateItem.FREE_AMMO, -0.1f);
        calculator.addParam(CalculateItem.RELOAD_TIME_MULTIPLIER, 0.999f);
        agentBasic.setFireInterval();
        agentBasic.setReloadTime();
        skillState = SkillState.ENDING;
    }

    private void Start()
    {
        calculator = GetComponent<BuffCalculator>();
        fireC = GetComponentInChildren<FireController>();
    }

    private void Update()
    {
        if (timer > 0f) timer -= Time.deltaTime;
        if (skillState == SkillState.ACTIVE && timer <= 0f)
        {
            endAddBuff();
        }
    }
}
