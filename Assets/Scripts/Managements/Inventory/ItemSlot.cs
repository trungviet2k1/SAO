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
        /*else
        {
            GameObject targetItem = Item;
            SwapItems(droppedItem, targetItem);
        }*/
    }

    /*private void SwapItems(GameObject item1, GameObject item2)
    {
        Transform item1Parent = item1.transform.parent;
        Transform item2Parent = item2.transform.parent;

        Vector2 item1Position = item1.transform.localPosition;
        Vector2 item2Position = item2.transform.localPosition;

        item1.transform.SetParent(item2Parent);
        item1.transform.localPosition = item2Position;

        item2.transform.SetParent(item1Parent);
        item2.transform.localPosition = item1Position;
    }*/
}