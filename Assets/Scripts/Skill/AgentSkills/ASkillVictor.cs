using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASkillVictor : SkillBasic
{
    public GameObject curTemp;
    public GameObject turret;
    private FortificationBasic fortiBasic;
    public override void startSummon()
    {
        if (curTemp == null)
        {
            Destroy(curTemp);
        }
        curTemp = Instantiate(turret);
        curTemp.transform.position = position;
        //curTemp.GetComponentInChildren<SkillEffectBasic>().dataSource = agentBasic;
        fortiBasic = curTemp.GetComponentInChildren<FortificationBasic>();
        fortiBasic.fortiData.damage = agentBasic.damage*0.5f;
        fortiBasic.fortiData.maxHP = agentBasic.maxHP;
        fortiBasic.setDamage();
        fortiBasic.setHP();
        curTemp.GetComponentInChildren<FortificationController>().currentHP = fortiBasic.maxHP;
        timer = 40f;
        skillState = SkillState.ACTIVE;
    }

    private void Update()
    {
        if (timer > 0f) timer -= Time.deltaTime;
        if (skillState == SkillState.ACTIVE && timer <= 0f)
        {
            Destroy(curTemp);
            skillState = SkillState.ENDING;
        }
    }
}
