using UnityEngine;

public enum ItemType
{
    Helmet,
    ChestArmor,
    Jewelry,
    Pants,
    Gloves,
    Boots,
    Weapon,
    Shield,
    None
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int itemID;
    public GameObject prefab;
    public string itemName;
    [TextArea(5, 10)]
    public string description;
    public int itemPrice;
    public float weight;
    public int requiredLevel;
    public ItemType itemType;
    public string prefab3DName;
}