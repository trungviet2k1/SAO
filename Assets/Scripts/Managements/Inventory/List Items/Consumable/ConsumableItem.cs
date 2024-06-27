using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Inventory/Consumable Item")]
public class ConsumableItem : ScriptableObject
{
    public GameObject prefabIcon;
    public string itemName;
    public string description;
    public float weight;
    public int itemPrice;

    public virtual void Use()
    {
        Debug.Log($"Using {itemName}");
    }
}