using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASkillDelta : SkillBasic
{
    BuffCalculator calculator;
    public override void startAddBuff()
    {
        timer = 10f;
        counter = 10000;
        skillState = SkillState.ACTIVE;
        calculator = GetComponent<BuffCalculator>();
        calculator.addParam(CalculateItem.SPEED_MULTIPLIER, 0.7f);
        agentBasic.setMoveSpeed();
    }

    public void minusCount()
    {
        counter--;
        if (counter == 0) endAddBuff();
    }

    public override void endAddBuff()
    {
        calculator.addParam(CalculateItem.SPEED_MULTIPLIER, -0.7f);
        agentBasic.setMoveSpeed();
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
