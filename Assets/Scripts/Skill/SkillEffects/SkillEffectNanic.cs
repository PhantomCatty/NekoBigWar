using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectNanic : SkillEffectBasic
{
    public float effectTimer;

    new void Start()
    {
        detector = GetComponentInChildren<SkillEffectDetector>();
        effectTimer = 0.1f;
        timer = 0.2f;
        startEffect();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        effectTimer -= Time.deltaTime;
        if (effectTimer <= 0f)
        {
            effectTimer = 1f;
            if (detector.targetList.Count != 0)
            {
                foreach (GameObject obj in detector.targetList)
                {
                    processTarget(obj);
                }
            }
        }
        if (timer <= 0f)
        {
            endEffect();
        }
    }

    public override void processTarget(GameObject obj)
    {
        //这里留位置触发一个事件,给被动技能的重伤加倍治疗效果
        //如果是这样的话,事件参数可能要同时包含目标和数值的"引用",以方便直接调整数值

        if (obj.tag == "Player")//指挥官
        {
            obj.GetComponentInChildren<NekoMove>().hurt(dataSource.damage * 1.5f, 0);
        }
        else//干员
        {
            obj.GetComponentInChildren<AgentController>().hurt(dataSource.damage * 1.5f, 0);
        }
    }

    public override void startEffect()
    {

    }

    public override void endEffect()
    {
        Destroy(gameObject);
    }
}
