using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
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

        if (!Item)
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.transform.localPosition = Vector2.zero;

            EquipSlot equipSlot = droppedItem.GetComponentInParent<EquipSlot>();
            if (equipSlot != null)
            {
                equipSlot.UnequipItem();
            }
        }
        else
        {
            droppedItem.transform.SetParent(transform, true);
            droppedItem.transform.position = Vector2.zero;
        }
    }
}