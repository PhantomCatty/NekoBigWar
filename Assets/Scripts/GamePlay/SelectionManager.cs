using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;

    public GameObject currentObj;

    public GameObject virtualCanvas;
    private Image agentHP;
    private Image agentAmmo;
    private Image targetFrontSight;

    private AgentController curAgentC;
    public GameObject circle;
    public GameObject detectorCircle;
    private bool isActive;
    bool onAssign;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        agentHP = virtualCanvas.transform.GetChild(7).GetComponent<Image>();
        agentAmmo = virtualCanvas.transform.GetChild(8).GetComponent<Image>();
        targetFrontSight = virtualCanvas.transform.GetChild(9).GetComponent<Image>();
        onAssign = false;
        isActive = false;
        virtualCanvas.SetActive(false);
        circle.gameObject.SetActive(false);
        detectorCircle.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == true)
        {
            if (currentObj)
            {
                virtualCanvas.transform.position = currentObj.transform.position;
                circle.transform.position = currentObj.transform.position;
                detectorCircle.transform.position = currentObj.transform.position;
                circle.GetComponent<DrawRange>().ToDrawCircle(currentObj.transform.position, curAgentC.agentBasic.range);
                detectorCircle.GetComponent<DrawRange>().ToDrawCircle(curAgentC.detector.transform.position, curAgentC.agentBasic.detectRange);
                agentHP.fillAmount = curAgentC.currentHP / curAgentC.agentBasic.maxHP;
                agentAmmo.fillAmount = (float)curAgentC.fireController.ammo / curAgentC.agentBasic.magazing;
                if(curAgentC.detector.currentTarget)
                {
                    targetFrontSight.gameObject.SetActive(true);
                    targetFrontSight.transform.position = curAgentC.detector.currentTarget.transform.position;
                }
                else
                {
                    targetFrontSight.gameObject.SetActive(false);
                }
            }
            else onQuit();
        }
    }

    public void onBackgroundClick()
    {
        if (isActive == true)
        {
            if (onAssign==true)
            {
                Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousepos.z = 0;
                curAgentC.point = mousepos;
                curAgentC.setStatus(AgentStatus.ASSIGN);
                onQuit();
            }
            else
            {
                onQuit();
            }

        }
    }

    public void onItemClick(GameObject obj)
    {
        currentObj = obj;
        curAgentC = obj.GetComponentInChildren<AgentController>();
        virtualCanvas.SetActive(true);
        virtualCanvas.transform.position = obj.transform.position;
        circle.SetActive(true);
        detectorCircle.SetActive(true);
        circle.transform.position = obj.transform.position;
        detectorCircle.transform.position = curAgentC.detector.transform.position;
        isActive = true;
    }

    public void onQuit()
    {
        this.virtualCanvas.SetActive(false);
        circle.SetActive(false);
        detectorCircle.SetActive(false);
        isActive = false;
        onAssign = false;
    }

    public void onStateBtnClicked(int state)
    {
        switch (state){
            case 0:
                {
                    curAgentC.setStatus(AgentStatus.STAND_HOLD);
                    break;
                }
            case 1:
                {
                    curAgentC.setStatus(AgentStatus.PATROL);
                    break;
                }
            case 2:
                {
                    curAgentC.setStatus(AgentStatus.FOCUS);
                    break;
                }
            case 3:
                {
                    onAssign = true;
                    break;
                }
            default:break;
        }
        if (state != 3)
        {
            onQuit();
        }

    }
}

public enum UIState
{
    NULL=0,
    AGENT_UI=1
}