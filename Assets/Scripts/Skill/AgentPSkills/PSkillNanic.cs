using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PSkillNanic : PassiveSkillBasic
{
    AgentAimDetector detector;
    UnityAction<GameObject> action;
    private void Start()
    {
        detector = GetComponentInChildren<AgentController>().detector;
        action += doubleHeal;
        GetComponentInChildren<FireController>().fireEvent.AddListener(action);
        startSkill();
    }
    public void doubleHeal(GameObject bullet)
    {
        if (detector.currentTarget.tag == "Ally" && detector.currentTarget.GetComponentInChildren<AgentController>().currentHP / detector.currentTarget.GetComponentInChildren<AgentBasic>().maxHP < 0.3f)
        {
            bullet.GetComponentInChildren<BulletBasic>().damage *= 2;
        }
    }
}