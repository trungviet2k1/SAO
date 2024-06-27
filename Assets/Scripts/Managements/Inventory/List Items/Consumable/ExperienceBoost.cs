using UnityEngine;

[CreateAssetMenu(fileName = "New Experience Boost", menuName = "Inventory/Consumable Item/ExperienceBoost")]
public class ExperienceBoost : ConsumableItem
{
    public float experienceMultiplier;
    public float duration;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Increased experience by {experienceMultiplier}x for {duration} seconds.");
    }
}