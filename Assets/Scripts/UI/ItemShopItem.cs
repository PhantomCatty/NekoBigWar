using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
    used for item slot in shop
*/
public class ItemShopItem : MonoBehaviour
{

    public ItemBasic itemData;
    public Image iconSprite;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemDescription;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateData(ItemBasic data)
    {
        itemData = data;
        iconSprite.sprite = itemData.itemIcon;
        itemName.text = itemData.itemName;
        itemPrice.text = itemData.price.ToString();
        itemDescription.text = itemData.description;
    }
}
