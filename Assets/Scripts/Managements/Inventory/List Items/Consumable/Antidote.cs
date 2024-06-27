using UnityEngine;

[CreateAssetMenu(fileName = "New Antidote", menuName = "Inventory/Consumable Item/Antidote")]
public class Antidote : ConsumableItem
{
    public override void Use()
    {
        base.Use();
        Debug.Log("Cured poison.");
    }
}