using UnityEngine;

public class ItemBasic : MonoBehaviour
{
    public ItemData itemData;

    // 物品的基本属性
    public string itemName;
    public Sprite itemIcon;
    public string description;        // 道具描述
    public int price;                 // 商店价格

    void Awake()
    {
        InitializeItem();
    }

    void InitializeItem()
    {
        itemName = itemData.itemName;
        itemIcon = itemData.icon;
        description = itemData.description;
        price = itemData.price;
    }
}