using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能效果物本质上就是一个目标侦测器+访问器
/// 从这点来讲,这个东西和agentcontroller没什么区别
/// </summary>
public class SkillEffectSpectia : SkillEffectBasic
{
    public float effectTimer;

    new void Start()
    {
        detector = GetComponentInChildren<SkillEffectDetector>();
        effectTimer = 0f;
        timer = 6f;
        startEffect();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        effectTimer -= Time.deltaTime;
        if (timer <= 0f)
        {
            endEffect();
        }
        if (effectTimer <= 0f)
        {
            effectTimer = 0.25f;
            if (detector.targetList.Count != 0)
            {
                foreach(GameObject obj in detector.targetList)
                {
                    processTarget(obj);
                }
            }
        }
    }

    public override void processTarget(GameObject obj)
    {
        obj.GetComponentInChildren<EnemyController>().hurt(dataSource.damage*0.05f, 0);
    }

    public override void startEffect()
    {

    }

    public override void endEffect()
    {
        //List<GameObject> temp = new List<GameObject>(detector.targetList);
        //detector.gameObject.SetActive(false);
        //foreach (GameObject obj in temp)
        //{
        //    endBuff(obj);
        //}
        Destroy(gameObject);
    }

    public void addBuff(GameObject obj)
    {
        obj.GetComponentInChildren<BuffCalculator>().addParam(CalculateItem.SPEED_FINALM,-0.8f);
        obj.GetComponentInChildren<EnemyBasic>().setMoveSpeed();
    }

    public void endBuff(GameObject obj)
    {
        obj.GetComponentInChildren<BuffCalculator>().addParam(CalculateItem.SPEED_FINALM, 0.8f);
        obj.GetComponentInChildren<EnemyBasic>().setMoveSpeed();
    }
}
