using UnityEngine;

[CreateAssetMenu(fileName = "New Speed Boost", menuName = "Inventory/Consumable Item/SpeedBoost")]
public class SpeedBoost : ConsumableItem
{
    public float speedMultiplier;
    public float duration;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Increased speed by {speedMultiplier}x for {duration} seconds.");
    }
}