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
    private ConsumableItem consumableItem;
    private ShopManager shopManager;

    public void SetupItemForSale(Item newItem, ShopManager manager)
    {
        item = newItem;
        shopManager = manager;
        itemImage.sprite = item.prefab.GetComponent<Image>().sprite;
        itemNameText.text = item.itemName;
        itemPriceText.text = item.itemPrice.ToString() + " Col";
        buyButton.onClick.AddListener(BuyItemButtonClicked);
        GetComponent<Button>().onClick.AddListener(OnItemClicked);
        GetComponent<Button>().onClick.AddListener(ButtonSoundManager.Instance.PlayButtonClickSound);
        buyButton.onClick.AddListener(ButtonSoundManager.Instance.PlayButtonClickSound);
    }

    public void SetupConsumableItemForSale(ConsumableItem newConsumableItem, ShopManager manager)
    {
        consumableItem = newConsumableItem;
        shopManager = manager;
        itemImage.sprite = consumableItem.prefabIcon.GetComponent<Image>().sprite;
        itemNameText.text = consumableItem.itemName;
        itemPriceText.text = consumableItem.itemPrice.ToString() + " Col";
        buyButton.onClick.AddListener(BuyConsumableItemButtonClicked);
        GetComponent<Button>().onClick.AddListener(OnConsumableClicked);
        GetComponent<Button>().onClick.AddListener(ButtonSoundManager.Instance.PlayButtonClickSound);
        buyButton.onClick.AddListener(ButtonSoundManager.Instance.PlayButtonClickSound);
    }

    void BuyItemButtonClicked()
    {
        shopManager.BuyItem(item);
    }

    void BuyConsumableItemButtonClicked()
    {
        shopManager.BuyConsumableItem(consumableItem);
    }

    void OnItemClicked()
    {
        shopManager.DisplayItemInformation(item);
    }

    void OnConsumableClicked()
    {
        shopManager.DisplayConsumableInformation(consumableItem);
    }
}