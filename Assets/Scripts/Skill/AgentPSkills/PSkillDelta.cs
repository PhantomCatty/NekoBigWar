using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSkillDelta : PassiveSkillBasic
{
    public BuffCalculator calculator;
    private float lastAddon;
    public override void startAddBuff()
    {
        lastAddon = 0f;
        calculator = GetComponent<BuffCalculator>();
        skillState = SkillState.ACTIVE;
        calculator.addParam(CalculateItem.ATTACK_ADDON, agentBasic.moveSpeed-lastAddon);
        lastAddon = agentBasic.moveSpeed;
        agentBasic.setDamage();
    }

    private void Start()
    {
        startSkill();
    }

    public void resetAddon()
    {
        calculator.addParam(CalculateItem.ATTACK_ADDON, agentBasic.moveSpeed - lastAddon);
        lastAddon = agentBasic.moveSpeed;
        Debug.Log(lastAddon);
        agentBasic.setDamage();
    }
}