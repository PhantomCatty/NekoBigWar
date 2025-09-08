using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerShop : MonoBehaviour
{
    public static UIControllerShop instance;

    public GameObject ItemSlots1;
    public GameObject ItemSlots2;
    public GameObject ItemSlots3;
    public List<AgentBasic> AgentList;
    public List<ItemBasic> ItemList;

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
}
