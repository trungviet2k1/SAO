using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Inventory/Consumable Item")]
public class ConsumableItem : ScriptableObject
{
    public int itemID;
    public GameObject prefabIcon;
    public string itemName;
    [TextArea(5, 10)]
    public string description;
    public float weight;
    public int requiredLevel;
    public int itemPrice;
    public bool stackable;
    public int maxStackCount;

    [HideInInspector] public int stackCount;

    public virtual void Use()
    {
        Debug.Log($"Using {itemName}");
    }
}