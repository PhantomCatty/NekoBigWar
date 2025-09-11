using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIControllerShop : MonoBehaviour
{
    public static UIControllerShop instance;

    public List<AgentBasic> AgentList;
    public List<ItemBasic> ItemList;
    public List<AgentShopItem> AgentSlotList;
    public List<ItemShopItem> ItemSlotList;
    public TextMeshProUGUI coinText;

    /*
        I am not sure whether put column and row interval here is a good idea
        'Interval' here means the distance between slots plus the size of the slot
    */
    public int ItemColumnInterval;
    public int ItemRowInterval;
    public int AgentSlotRowInterval;
    public int ItemColNum;

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

        gameObject.SetActive(false);
    }

    //refresh data when enabled
    void OnEnable()
    {
        Refresh();
    }

    void Refresh()
    {
        coinText.text = InGameData.instance.curCoin.ToString();
        // Refresh the shop UI with the current item list
        UpdateItemSlots();
        UpdateAgentSlots();
    }

    void UpdateItemSlots()
    {
        // 根据ItemList数量显示对应数量的ItemSlot，并隐藏多余的ItemSlot
        for (int i = 0; i < ItemSlotList.Count; i++)
        {
            if (i < ItemList.Count)
            {
                ItemSlotList[i].gameObject.SetActive(true);
                ItemSlotList[i].UpdateData(ItemList[i]);
            }
            else
            {
                ItemSlotList[i].gameObject.SetActive(false);
            }
        }
    }

    void UpdateAgentSlots()
    {

        // 根据AgentList数量显示对应数量的AgentSlot，并隐藏多余的AgentSlot
        for (int i = 0; i < AgentSlotList.Count; i++)
        {
            if (i < AgentList.Count)
            {
                AgentSlotList[i].gameObject.SetActive(true);
                AgentSlotList[i].UpdateData(AgentList[i]);
            }
            else
            {
                AgentSlotList[i].gameObject.SetActive(false);
            }
        }
    }
}
