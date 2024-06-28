using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Inventory/Consumable Item")]
public class ConsumableItem : ScriptableObject
{
    public int itemID;
    public GameObject prefabIcon;
    public string itemName;
    public string description;
    public float weight;
    public int itemPrice;
    public bool stackable;
    public int maxStackCount;

    [HideInInspector] public int stackCount;

    public virtual void Use()
    {
        Debug.Log($"Using {itemName}");
    }
}