using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASkillHawk : SkillBasic
{
    public override void startAddBuff()
    {
        timer = 6f;
        skillState = SkillState.ACTIVE;
        targetList[0].GetComponentInChildren<BuffCalculator>().multiplyParam(CalculateItem.DAMAGE_REDUCTION, 1.3f);
    }

    public override void endAddBuff()
    {
        skillState = SkillState.ENDING;
        if(targetList[0])
            targetList[0].GetComponentInChildren<BuffCalculator>().multiplyParam(CalculateItem.DAMAGE_REDUCTION, -1.3f);
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
