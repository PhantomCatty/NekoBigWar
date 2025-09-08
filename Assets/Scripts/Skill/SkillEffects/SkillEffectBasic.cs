using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能效果物本质上就是一个目标侦测器+访问器
/// 从这点来讲,这个东西和agentcontroller没什么区别
/// </summary>
public class SkillEffectBasic : MonoBehaviour
{
    public float timer;
    public float counter;
    public AgentAimDetector detector;
    public AgentBasic dataSource;
    // Start is called before the first frame update
    protected void Start()
    {
        detector = GetComponentInChildren<AgentAimDetector>();
        startEffect();
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

    public virtual void processTarget(GameObject obj)
    {
        
    }

    public virtual void startEffect()
    {

    }

    public virtual void endEffect()
    {
        Destroy(gameObject);
    }
}
