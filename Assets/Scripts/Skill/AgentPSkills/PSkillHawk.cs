using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSkillHawk : PassiveSkillBasic
{
    public AgentAimDetector agentC;
    public GameObject trap;

    private void Start()
    {
        timer = 10f;
        agentC = GetComponentInChildren<AgentAimDetector>();    
        startSkill();  
    }


    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            timer = 10f;
            effect();
        }
    }
    public void effect()
    {
        Instantiate(trap);
        if (agentC.currentTarget)
        {

            trap.transform.position = agentC.transform.position;
        }
        else
        {
            float angle = Random.Range(-Mathf.PI, Mathf.PI);
            float radius = Random.Range(0f, agentC.GetComponent<CircleCollider2D>().radius);
            Vector3 vec = new Vector3(radius * Mathf.Cos(angle),  radius * Mathf.Sin(angle), 0f);
            trap.transform.position = agentC.transform.position + vec;
        }
        trap.GetComponent<SkillEffectBasic>().dataSource = agentBasic;
    }
}