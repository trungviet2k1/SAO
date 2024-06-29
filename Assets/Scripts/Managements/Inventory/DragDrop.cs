using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    public static Vector3 startPosition;
    public static Transform startParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;

        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(startParent.transform.parent.transform.parent);
        itemBeingDragged = gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;

        if (transform.parent == startParent || transform.parent == startParent.transform.parent.transform.parent)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (startParent.TryGetComponent<EquipSlot>(out var equipSlot))
        {
            equipSlot.UnequipItem();
        }
    }
}