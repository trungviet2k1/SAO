using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonColorController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Button button;
    private Color normalColor = Color.white;
    private Color highlightedColor = new Color32(224, 220, 33, 255);
    private Color pressedColor = new Color32(151, 130, 46, 255);

    void Start()
    {
        button = GetComponent<Button>();
        SetButtonColor(normalColor);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetButtonColor(highlightedColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetButtonColor(normalColor);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetButtonColor(pressedColor);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetButtonColor(highlightedColor);
    }

    private void SetButtonColor(Color color)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.highlightedColor = highlightedColor;
        cb.pressedColor = pressedColor;
        cb.selectedColor = highlightedColor;
        button.colors = cb;
    }
}