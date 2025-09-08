using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SlotItem defines what state the slot is in and what should it do
/// that is, the class includes some function and procedure to deal with simple tasks 
/// </summary>
public class SlotItem : MonoBehaviour
{
    public int slotIndex;
    public SlotStatus slotStatus;
    public Image iconImage;
    public Image maskImage;
    public float CDTime;
    public float timer;
    public AgentController curObj;//change ref between prefab and instance in game depends on the status 
    public GameObject agentInstance;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        iconImage = GetComponent<Image>();
        maskImage = transform.GetChild(0).GetComponent<Image>();
        slotStatus = SlotStatus.AGENT;
        maskImage.fillAmount = 0f;
        curObj = agentInstance.GetComponentInChildren<AgentController>();
        iconImage.sprite = curObj.agentBasic.agentIcon;
    }

    // Update is called once per frame
    void Update()
    {
        switch (slotStatus)
        {
            case SlotStatus.AGENT_CD:
                {
                    if (timer > 0f)
                    {
                        timer -= Time.deltaTime;
                        maskImage.fillAmount = timer / CDTime;
                        if (timer <= 0f) setSlotStatus(SlotStatus.AGENT);
                    }
                    break;
                }
            case SlotStatus.SKILL_CD:
                {
                    if (timer > 0f)
                    {
                        timer -= Time.deltaTime;
                        maskImage.fillAmount = timer / CDTime;
                        if (timer <= 0f) setSlotStatus(SlotStatus.SKILL);
                    }
                    break;
                }
            default:break;
        }
    }

    public void setSlotStatus(SlotStatus status)
    {
        slotStatus = status;
        if (status == SlotStatus.AGENT_CD)//干员下场,进入复活CD,切换到预制体引用,切换图片
        {
            curObj = agentInstance.GetComponentInChildren<AgentController>();
            iconImage.sprite = curObj.agentBasic.agentIcon;
            timer = CDTime = curObj.agentBasic.respawnTime;
            maskImage.fillAmount = 1;
        }
        else if (status == SlotStatus.SKILL_CD) {//干员释放技能,槽位切换为SKILL_CD

            CDTime = curObj.agentBasic.skillCD;
            timer = CDTime = curObj.agentBasic.skillCD;
            maskImage.fillAmount = 1;
        }
        else if (status == SlotStatus.SKILL)//干员上场或者冷却完毕进入SKILL,切换引用为场上干员,切换图片
        {
            curObj=InGameData.instance.agentList[slotIndex].GetComponentInChildren<AgentController>();
            iconImage.sprite = curObj.agentBasic.skillIcon;
        }
        //复活cd结束进入Agent状态,无变化

        //Disable,设置按钮为disable,一般来说不会用到
    }

    public void onCostChanged(float cost)
    {
        if (slotStatus == SlotStatus.AGENT)
        {
            if (button.interactable==true && cost < curObj.agentBasic.AssignCost)
            {
                button.interactable = false;
            }
            else if (button.interactable == false && cost > curObj.agentBasic.AssignCost)
            {
                button.interactable = true;
            }
        }
        else if (slotStatus == SlotStatus.SKILL)
        {
            if (button.interactable == true && cost < curObj.agentBasic.SkillCost)
            {
                button.interactable = false;
            }
            else if (button.interactable == false && cost > curObj.agentBasic.SkillCost)
            {
                button.interactable = true;
            }
        }
    }
}

public enum SlotStatus
{
    DISABLE=0,
    AGENT=1,
    AGENT_CD=2,
    SKILL=3,
    SKILL_CD=4
}