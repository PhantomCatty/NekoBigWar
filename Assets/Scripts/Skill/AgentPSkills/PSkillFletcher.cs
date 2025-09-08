using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSkillFletcher : PassiveSkillBasic
{
    public BuffCalculator calculator;
    public int buffCounter;

    private void Start()
    {
        startSkill();
    }

    public override void startAddBuff()
    {
        calculator = GetComponent<BuffCalculator>();
        skillState = SkillState.ACTIVE;
        calculator.addParam(CalculateItem.FREE_AMMO, 0.2f);
    }
}
