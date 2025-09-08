using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASkillBarrel : SkillBasic
{
    BuffCalculator calculator;
    public override void startAddBuff()
    {
        timer = 10f;
        counter = 1;
        skillState = SkillState.ACTIVE;
        calculator = GetComponent<BuffCalculator>();
        calculator.addParam(CalculateItem.ATTACK_MULTIPLIER, 2);
        calculator.addParam(CalculateItem.ARMOR_PIERCE_ADDON, 900);
        agentBasic.setDamage();
        agentBasic.setArmorPierce();
    }

    public void minusCount()
    {
        counter--;
        if (counter == 0) endAddBuff();
    }

    public override void endAddBuff()
    {
        calculator.addParam(CalculateItem.ATTACK_MULTIPLIER, -2);
        calculator.addParam(CalculateItem.ARMOR_PIERCE_ADDON, -900);
        agentBasic.setDamage();
        agentBasic.setArmorPierce();
        skillState = SkillState.ENDING;
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
