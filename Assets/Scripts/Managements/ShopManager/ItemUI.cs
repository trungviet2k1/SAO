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

    private void OnEnable()
    {
        CurrencyManager.Instance.OnMoneyChanged += UpdateBuyButtonState;
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnMoneyChanged -= UpdateBuyButtonState;
        }
    }

    public void SetupItemForSale(Item newItem, ShopManager manager)
    {
        item = newItem;
        shopManager = manager;
        itemImage.sprite = item.prefab.GetComponent<Image>().sprite;
        itemNameText.text = item.itemName;
        itemPriceText.text = $"{item.itemPrice} Col";
        buyButton.onClick.AddListener(BuyItemButtonClicked);
        GetComponent<Button>().onClick.AddListener(OnItemClicked);
        UpdateBuyButtonState(0);
    }

    public void SetupConsumableItemForSale(ConsumableItem newConsumableItem, ShopManager manager)
    {
        consumableItem = newConsumableItem;
        shopManager = manager;
        itemImage.sprite = consumableItem.prefabIcon.GetComponent<Image>().sprite;
        itemNameText.text = consumableItem.itemName;
        itemPriceText.text = $"{consumableItem.itemPrice} Col";
        buyButton.onClick.AddListener(BuyConsumableItemButtonClicked);
        GetComponent<Button>().onClick.AddListener(OnConsumableClicked);
        UpdateBuyButtonState(0);
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

    void UpdateBuyButtonState(int _)
    {
        if (item != null)
        {
            buyButton.interactable = CurrencyManager.Instance.CanAfford(item.itemPrice) && CharacterLevelSystem.Instance.Level >= item.requiredLevel;
        }
        else if (consumableItem != null)
        {
            buyButton.interactable = CurrencyManager.Instance.CanAfford(consumableItem.itemPrice) && CharacterLevelSystem.Instance.Level >= consumableItem.requiredLevel;
        }
    }
}