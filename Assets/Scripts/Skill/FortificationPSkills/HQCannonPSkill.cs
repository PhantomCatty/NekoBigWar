using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQCannonPSkill : PassiveSkillBasic
{
    public BuffCalculator calculator;
    public int buffCounter;
    public float cooldown;

    void Start()
    {
        buffCounter = 0;
        cooldown = 3f;
        if (!calculator)
        {
            calculator = GetComponent<BuffCalculator>();
        }
        fortificationBasic = GetComponent<FortificationBasic>();
        timer = cooldown;
    }

    public override void startAddBuff()
    {
        skillState = SkillState.ACTIVE;
        buffCounter = 0;
        timer = cooldown;
    }

    private void Update()
    {
        if (skillState == SkillState.ACTIVE)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = cooldown;
                if (buffCounter < 5)
                {
                    buffCounter++;
                    calculator.addParam(CalculateItem.ATTACK_ADDON, 8);
                    calculator.addParam(CalculateItem.MAGIC_ATTACK_ADDON, 5);
                    fortificationBasic.setDamage();
                }
            }
        }
    }

    public override void endAddBuff()
    {
        skillState = SkillState.ENDING;
        calculator.addParam(CalculateItem.ATTACK_ADDON, -8 * buffCounter);
        calculator.addParam(CalculateItem.MAGIC_ATTACK_ADDON, -5 * buffCounter);
        buffCounter = 0;
        fortificationBasic.setDamage();
    }
}
