using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能效果物本质上就是一个目标侦测器+访问器
/// 从这点来讲,这个东西和agentcontroller没什么区别
/// </summary>
public class PSkillEffectHawk : SkillEffectBasic
{
    new void Start()
    {
        timer = 300f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            endEffect();
        }
    }

    public override void processTarget(GameObject obj)
    {
        obj.GetComponentInChildren<EnemyController>().hurt(dataSource.damage * 1.6f, 0, dataSource.armorPierce, dataSource.mArmorPierce);
        obj.GetComponentInChildren<BuffCalculator>().addTimer(new BuffTimer(CalculateItem.SPEED_FINALM, -1f, 1f, true));
        obj.GetComponentInChildren<EnemyBasic>().setMoveSpeed();
        Destroy(gameObject);
    }

    public override void endEffect()
    {
        Destroy(gameObject);
    }
}