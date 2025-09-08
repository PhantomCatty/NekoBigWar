using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 技能检测器,多的是进入和离开范围时触发的事件
/// </summary>
public class SkillEffectDetector : AgentAimDetector
{
    public UnityEvent<GameObject> targetInEvent;
    public UnityEvent<GameObject> targetOutEvent;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && CommonTools.compareTarget(targetType, TargetType.ENEMY))
        {
            if (targetList.Count == 0)
            {
                currentTarget = other.gameObject;
            }
            if (!targetList.Find(obj => obj == other.gameObject))
                targetList.Add(other.gameObject);
            targetInEvent.Invoke(other.gameObject);
        }
        else if ((other.tag == "Agent" || other.tag == "Player") && CommonTools.compareTarget(targetType, TargetType.ALLY))
        {
            if (targetList.Count == 0)
            {
                currentTarget = other.gameObject;
            }
            if (!targetList.Find(obj => obj == other.gameObject))
                targetList.Add(other.gameObject);
            targetInEvent.Invoke(other.gameObject);
        }
        else if (other.tag == "Enemy_Fortification" && CommonTools.compareTarget(targetType, TargetType.ENEMY_FORTIFICATION))
        {
            if (targetList.Count == 0)
            {
                currentTarget = other.gameObject;
            }
            if (!targetList.Find(obj => obj == other.gameObject))
                targetList.Add(other.gameObject);
            targetInEvent.Invoke(other.gameObject);
        }
        else if (other.tag == "Ally_Fortification" && CommonTools.compareTarget(targetType, TargetType.ALLY_FORTIFICATION))
        {
            if (targetList.Count == 0)
            {
                currentTarget = other.gameObject;
            }
            if (!targetList.Find(obj => obj == other.gameObject))
                targetList.Add(other.gameObject);
            targetInEvent.Invoke(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //procedure:find obj in the list, remove it;
        if (targetList.Contains(other.gameObject))
        {
            targetList.Remove(other.gameObject);
            targetOutEvent.Invoke(other.gameObject);
            if (currentTarget == other.gameObject)
            {
                findTarget();
            }
        }
    }
}