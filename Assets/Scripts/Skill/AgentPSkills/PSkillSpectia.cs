using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSkillSpectia : PassiveSkillBasic
{
    private AgentController agentC;
    public override void startAddBuff()
    {
        timer = 0f;
        agentC = GetComponent<AgentController>();
        skillState = SkillState.ACTIVE;
    }

    private void Start()
    {
        startSkill();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            timer = 1f;
            agentC.hurt(-4, 0);
        }
    }
}