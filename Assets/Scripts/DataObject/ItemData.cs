using UnityEngine;

/// <summary>
/// Item是可以被玩家获取的道具，具有多样的作用。所有Item都可以在商店购买，并持有在物品栏中。
/// </summary>
[CreateAssetMenu(fileName = "ItemData", menuName = "NekoBigWar/Data")]
public class ItemData : ScriptableObject
{
    [Header("基础属性")]
    public string itemName;           // 道具名称
    [TextArea]
    public string description;        // 道具描述
    public Sprite icon;               // 道具图标
    public int price;                 // 商店价格

    [Header("道具类型")]
    public ItemType itemType;         // 道具类型（能力提升、数值改变、经验加成、主动技能等）

    // 可扩展：道具的具体效果
    // 例如：提升Agent能力、改变Enemy数值、增加经验、提供主动技能等
    // 可通过继承或接口实现具体效果

    /// <summary>
    /// 使用道具时的效果（可重写或扩展）
    /// </summary>
    public virtual void Use(GameObject user)
    {
        // 实现道具的具体效果
        Debug.Log($"{itemName} 被 {user.name} 使用。");
    }
}

/// <summary>
/// 道具类型枚举
/// </summary>
public enum ItemType
{
    AbilityBoost,    // 提升能力
    EnemyModifier,   // 改变敌人属性
    CostBonus,       // 增加cost获取
    ExperienceBonus, // 增加经验获取
    ActiveSkill,     // 提供主动技能
    Other            // 其他
}