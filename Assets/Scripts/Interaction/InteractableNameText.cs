using TMPro;
using UnityEngine;

public class InteractableNameText : MonoBehaviour
{
    TextMeshProUGUI text;
    Transform cameraTransform;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        cameraTransform = Camera.main.transform;
        HideText();
    }

    public void ShowText(Interactable interactable)
    {
        text.text = GetInteractableName(interactable);
    }

    private string GetInteractableName(Interactable interactable)
    {
        if (interactable is PickUpItem)
        {
            return interactable.interactableName + "\n [E] Nhặt";
        }
        else if (interactable is InteractableChest)
        {
            InteractableChest chest = interactable as InteractableChest;
            return chest.isOpen ? interactable.interactableName + "\n [E] Đóng" : interactable.interactableName + "\n [E] Mở";
        }
        else if (interactable is InteractableLoot)
        {
            return interactable.interactableName + "\n [E] Loot";
        }
        else if (interactable is InteractableNPC)
        {
            return interactable.interactableName + "\n [E] Nói chuyện";
        }
        else
        {
            return interactable.interactableName;
        }
    }

    public void HideText()
    {
        text.text = "";
    }

    public void SetInteractableNamePosition(Interactable interactable)
    {
        if (interactable.TryGetComponent(out Collider collider))
        {
            transform.position = interactable.transform.position + Vector3.up * collider.bounds.size.y;
            transform.LookAt(2 * transform.position - cameraTransform.position);
        }
        else
        {
            Debug.LogError("Error: No collider found on interactable object.");
        }
    }
}