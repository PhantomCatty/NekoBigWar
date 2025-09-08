using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASkillNanic : SkillBasic
{
    public GameObject healer;

    public override void startEffect()
    {
        GameObject temp = Instantiate(healer);
        temp.transform.position = position;
        temp.GetComponentInChildren<SkillEffectBasic>().dataSource = agentBasic;
        timer = 0.1f;
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
