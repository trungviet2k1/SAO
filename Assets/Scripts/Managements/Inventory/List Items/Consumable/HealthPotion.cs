using UnityEngine;

public enum HealthPotionType
{
    Small,
    Medium,
    Great,
    Super,
    Miraculous
}

[CreateAssetMenu(fileName = "New Health Potion", menuName = "Inventory/Consumable Item/HealthPotion")]
public class HealthPotion : ConsumableItem
{
    public HealthPotionType potionType;
    public int healthRestoreAmount;

    public override void Use()
    {
        base.Use();
        HealthSystem healthSystem = HealthSystem.Instance;
        if (healthSystem == null)
        {
            Debug.LogError("HealthSystem instance is null. Cannot restore health.");
            return;
        }

        switch (potionType)
        {
            case HealthPotionType.Small:
                healthSystem.RestoreHealth(25);
                break;
            case HealthPotionType.Medium:
                healthSystem.RestoreHealth(45);
                break;
            case HealthPotionType.Great:
                healthSystem.RestoreHealth(75);
                break;
            case HealthPotionType.Super:
                healthSystem.RestoreHealth(150);
                break;
            case HealthPotionType.Miraculous:
                healthSystem.RestoreHealth(healthSystem.maxHealth);
                break;
            default:
                Debug.LogError($"Unhandled HealthPotionType: {potionType}");
                break;
        }

        Debug.Log($"Restored health with {potionType} potion.");
    }
}