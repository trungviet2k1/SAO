using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Inventory/Consumable Item")]
public class ConsumableItem : ScriptableObject
{
    public GameObject prefabIcon;
    public string itemName;
    public string description;
    public float weight;
    public int healthRestored;
    public int manaRestored;

    public void Use()
    {
        // Implement the logic for using the consumable item
        Debug.Log($"Using {itemName}. Restored {healthRestored} health and {manaRestored} mana.");
    }
}