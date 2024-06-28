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
    public int itemPrice;
    public float weight;
    public ItemType itemType;
    public string prefab3DName;
}