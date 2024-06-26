using UnityEngine;

public class PickUpItem : Interactable
{
    [Header("Item Data")]
    [SerializeField] WeaponItem item;

    public override void Start()
    {
        base.Start();
        interactableName = item.itemName;
    }

    protected override void Interaction()
    {
        base.Interaction();
        InventoryManager.Instance.AddItem(item);
        Destroy(gameObject);
    }
}