using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PSkillBarrel : PassiveSkillBasic
{
    public BuffCalculator calculator;
    public int buffCounter;
    public override void startAddBuff()
    {
        if (agentBasic.agentStatus == AgentStatus.STAND_HOLD)
        {
            skillState = SkillState.ACTIVE;
            calculator = GetComponent<BuffCalculator>();
            buffCounter = 0;
            timer = 15f;
        }
    }
    
    private void Start()
    {
        startSkill();
    }

    private void Update()
    {
        if (skillState == SkillState.ACTIVE)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 10f;
                if (buffCounter < 5)
                {
                    buffCounter++;
                    calculator.addParam(CalculateItem.RANGE_MULTIPLIER, 0.05f);
                    agentBasic.setRange();
                    agentBasic.GetComponent<AgentController>().setDetectRange(agentBasic.range);
                }
            }
        }
    }

    public override void endAddBuff()
    {
        Debug.Log(agentBasic);
        if (agentBasic.agentStatus != AgentStatus.STAND_HOLD)
        {
            skillState = SkillState.ENDING;
            calculator.addParam(CalculateItem.RANGE_MULTIPLIER, -0.05f * buffCounter);
            buffCounter = 0;
            agentBasic.setRange();
            agentBasic.GetComponent<AgentController>().setDetectRange(agentBasic.range);
        }
    }
}
