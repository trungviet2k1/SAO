using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IDropHandler
{
    public ItemType equipType;
    public Item equippedItem;

    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = DragDrop.itemBeingDragged;
        
        if (droppedItem.TryGetComponent<ItemComponent>(out var itemComponent))
        {
            Item item = itemComponent.item;

            if (item.itemType == equipType)
            {
                if (!Item)
                {
                    DragDrop.itemBeingDragged.transform.SetParent(transform);
                    DragDrop.itemBeingDragged.transform.localPosition = Vector2.zero;

                    if (equipType == ItemType.Weapon)
                    {
                        GameObject weaponPrefab = PrefabManager.Instance.GetWeaponPrefab(item.prefab3DName);
                        if (weaponPrefab != null)
                        {
                            EquipmentSystem.Instance.EquipWeapon(weaponPrefab);
                        }
                    }
                    InventoryManager.Instance.RemoveItem(item);
                }
                equippedItem = item;
            }
        }
        else
        {
            ConsumableItemComponent consumable = droppedItem.GetComponent<ConsumableItemComponent>();
            ConsumableItem consumableItem = consumable.consumableItem;

            if (consumableItem.itemType == ItemType.Consumable)
            {
                droppedItem.transform.SetParent(DragDrop.startParent);
                droppedItem.transform.position = DragDrop.startPosition;
                return;
            }
        }
    }

    public void UnequipItem()
    {
        if (equippedItem != null)
        {
            InventoryManager.Instance.ReturnItems(equippedItem);

            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }

            EquipmentSystem.Instance.UnEquipWeapon();
            equippedItem = null;
        }
    }
}