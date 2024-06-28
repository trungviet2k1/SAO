using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Shop UI")]
    public GameObject shopPanel;
    public TextMeshProUGUI shopNameText;

    [Header("List Items")]
    public Transform itemsParent;
    public GameObject itemPrefab;
    public List<Item> weaponItemsForSale;
    public List<Item> equipmentItemsForSale;
    public List<ConsumableItem> consumableItemsForSale;

    [Header("Item Information")]
    public ItemInfoPanel itemInfoPanel;
    public ConsumableInfoPanel consumableInfoPanel;

    [Header("Button Controls")]
    public Button closeButton;

    [HideInInspector] public bool isOpenShop = false;

    private ShopType currentShopType;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        shopPanel.SetActive(false);
        itemInfoPanel.gameObject.SetActive(false);
        consumableInfoPanel.gameObject.SetActive(false);
    }

    void Start()
    {
        closeButton.onClick.AddListener(CloseShop);
    }

    void PopulateShop<T>(List<T> itemsForSale) where T : ScriptableObject
    {
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (T item in itemsForSale)
        {
            GameObject itemGO = Instantiate(itemPrefab, itemsParent);
            ItemUI itemUI = itemGO.GetComponent<ItemUI>();
            if (item is Item itemObj)
            {
                itemUI.SetupItemForSale(itemObj, this);
            }
            else if (item is ConsumableItem consumableItemObj)
            {
                itemUI.SetupConsumableItemForSale(consumableItemObj, this);
            }
        }
    }

    public void BuyItem(Item item)
    {
        if (item != null 
            && CurrencyManager.Instance.CanAfford(item.itemPrice) 
            && CharacterLevelSystem.Instance.Level >= item.requiredLevel)
        {
            CurrencyManager.Instance.SubtractMoney(item.itemPrice);
            InventoryManager.Instance.AddItem(item);
        }
    }

    public void BuyConsumableItem(ConsumableItem consumableItem)
    {
        if (consumableItem != null 
            && CurrencyManager.Instance.CanAfford(consumableItem.itemPrice) 
            && CharacterLevelSystem.Instance.Level >= consumableItem.requiredLevel)
        {
            CurrencyManager.Instance.SubtractMoney(consumableItem.itemPrice);
            InventoryManager.Instance.AddConsumableItem(consumableItem);
        }
    }

    public void CloseShop()
    {
        isOpenShop = false;
        shopPanel.SetActive(false);
        itemInfoPanel.gameObject.SetActive(false);
        consumableInfoPanel.gameObject.SetActive(false);
    }

    public void OpenShop(ShopType shopType, string shopName)
    {
        currentShopType = shopType;
        isOpenShop = true;
        shopPanel.SetActive(true);
        shopNameText.text = shopName;

        switch (shopType)
        {
            case ShopType.WeaponShop:
                PopulateShop(weaponItemsForSale);
                break;
            case ShopType.EquipmentShop:
                PopulateShop(equipmentItemsForSale);
                break;
            case ShopType.ConsumableShop:
                PopulateShop(consumableItemsForSale);
                break;
        }
    }

    public void DisplayItemInformation(Item item)
    {
        itemInfoPanel.Display(item);
    }

    public void DisplayConsumableInformation(ConsumableItem consumableItem)
    {
        consumableInfoPanel.Display(consumableItem);
    }
}