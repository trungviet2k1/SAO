using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
    public Button buyButton;
    private Item item;
    private ShopManager shopManager;

    public void Setup(Item newItem, ShopManager manager)
    {
        item = newItem;
        shopManager = manager;
        itemImage.sprite = item.prefab.GetComponent<Image>().sprite;
        itemNameText.text = item.itemName;
        itemPriceText.text = item.itemPrice.ToString() + " Col";

        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    void OnBuyButtonClicked()
    {
        shopManager.BuyItem(item);
    }
}