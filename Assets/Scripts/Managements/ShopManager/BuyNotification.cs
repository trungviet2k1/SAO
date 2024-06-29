using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BuyNotification : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemNameText;
    public float displayDuration;
    public float destroyDelay = 1f;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void ShowNotification(Sprite sprite, string itemName)
    {
        itemImage.sprite = sprite;
        itemNameText.text = itemName;
        rectTransform.localPosition = Vector3.zero;
        gameObject.SetActive(true);
        StartCoroutine(NotificationRoutine());
    }

    private IEnumerator NotificationRoutine()
    {
        float elapsedTime = 0f;
        Vector3 startPos = rectTransform.localPosition;
        Vector3 endPos = startPos + Vector3.up * 65f;

        while (elapsedTime < displayDuration)
        {
            float t = elapsedTime / displayDuration;
            rectTransform.localPosition = Vector3.Lerp(startPos, endPos, t);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        HideNotification();
    }

    private void HideNotification()
    {
        canvasGroup.alpha = 0f;
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}