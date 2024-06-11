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
        Item item = droppedItem.GetComponent<ItemComponent>().item;

        if (item.itemType == equipType)
        {
            if (!Item)
            {
                droppedItem.transform.SetParent(transform);
                droppedItem.transform.localPosition = Vector2.zero;

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

    public void UnequipItem()
    {
        if (equippedItem != null)
        {
            InventoryManager.Instance.AddItem(equippedItem);

            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }
}