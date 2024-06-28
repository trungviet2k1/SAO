using TMPro;
using UnityEngine;

public class ConsumableItemComponent : MonoBehaviour
{
    public ConsumableItem consumableItem;
    public TextMeshProUGUI stackCountText;

    void Start()
    {
        if (stackCountText == null)
        {
            stackCountText = GetComponentInChildren<TextMeshProUGUI>();
        }
        UpdateStackCount();
    }

    public void UpdateStackCount()
    {
        if (consumableItem != null && consumableItem.stackable)
        {
            stackCountText.text = consumableItem.stackCount.ToString();
        }
    }
}