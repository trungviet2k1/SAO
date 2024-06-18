using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject arrow;
    private TextMeshProUGUI buttonText;
    private Color originalColor;

    void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            originalColor = buttonText.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (arrow != null)
        {
            arrow.SetActive(true);
        }
        if (buttonText != null)
        {
            buttonText.color = Color.white;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (arrow != null)
        {
            arrow.SetActive(false);
        }
        if (buttonText != null)
        {
            buttonText.color = originalColor;
        }
    }
}