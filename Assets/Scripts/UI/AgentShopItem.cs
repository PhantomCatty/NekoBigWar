using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
    used for item slot in shop
*/
public class AgentShopItem : MonoBehaviour
{
    public AgentBasic agentData;
    public Image iconSprite;
    public TextMeshProUGUI agentName;
    public TextMeshProUGUI agentPrice;
    public Image agentLevelBar;
    public TextMeshProUGUI agentLevel;
    public float agentAttack;
    public float agentArmor;
    public float agentHP;
    public float agentMArmor;
    public float agentFireInterval;
    public float agentMoveSpeed;
    public float agentRange;
    public TextMeshProUGUI textAttack;
    public TextMeshProUGUI textArmor;
    public TextMeshProUGUI textHP;
    public TextMeshProUGUI textMArmor;
    public TextMeshProUGUI textFireInterval;
    public TextMeshProUGUI textMoveSpeed;
    public TextMeshProUGUI textRange;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateData(AgentBasic newAgentData)
    {
        agentData = newAgentData;
        if (agentData != null)
        {
            // Update stats
            agentAttack = agentData.damage;
            agentArmor = agentData.armorPierce;
            agentHP = agentData.maxHP;
            agentMArmor = agentData.mArmorPierce;
            agentFireInterval = agentData.fireInterval;
            agentMoveSpeed = agentData.moveSpeed;
            agentRange = agentData.range;

            //update stats text
            textAttack.text = agentAttack.ToString("F1");
            textArmor.text = agentArmor.ToString("F1");
            textHP.text = agentHP.ToString("F1");
            textMArmor.text = agentMArmor.ToString("F1");
            textFireInterval.text = agentFireInterval.ToString("F1");
            textMoveSpeed.text = agentMoveSpeed.ToString("F1");
            textRange.text = agentRange.ToString("F1");

            //update UI
            iconSprite.sprite = agentData.agentData.agentIcon;
            agentName.text = agentData.agentData.agentName;
            agentPrice.text = agentData.agentData.upgradeCost[agentData.curLevel - 1].ToString();
            agentLevel.text = "Lv." + agentData.curLevel.ToString();
            agentLevelBar.fillAmount = (float)agentData.curLevel / (float)agentData.agentData.maxLevel;
        }
        else
        {
            // Clear the slot if no data
            iconSprite.sprite = null;
            agentName.text = "Empty";
            agentPrice.text = "";
            agentLevel.text = "";
            agentLevelBar.fillAmount = 0;

            // Clear stats
            agentAttack = 0;
            agentArmor = 0;
            agentHP = 0;
            agentMArmor = 0;
            agentFireInterval = 0;
            agentMoveSpeed = 0;
            agentRange = 0;
        }
    }
}
