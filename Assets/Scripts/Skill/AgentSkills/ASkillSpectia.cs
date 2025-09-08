using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASkillSpectia : SkillBasic
{
    public GameObject thorns;

    public override void startEffect()
    {
        GameObject temp = Instantiate(thorns);
        temp.transform.position = position;
        temp.GetComponentInChildren<SkillEffectBasic>().dataSource = agentBasic;
        timer = 6f;
    }

    private void Update()
    {
        if (timer > 0f) timer -= Time.deltaTime;
        if (skillState == SkillState.ACTIVE && timer <= 0f)
        {
            skillState = SkillState.ENDING;
        }
    }
}
