using UnityEngine;

[CreateAssetMenu(fileName = "InventorySettings", menuName = "Inventory/Settings")]
public class InventorySettings : ScriptableObject
{
    public int maxSlots = 35;
    public float maxBagWeight = 300f;
}