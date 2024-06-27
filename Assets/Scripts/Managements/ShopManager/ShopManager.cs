using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

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
    public GameObject itemInformationPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemHPBonusText;
    public TextMeshProUGUI itemAttackPowerText;
    public TextMeshProUGUI itemDefensePowerText;

    [Header("Consumable Information")]
    public GameObject consumableInformationPanel;
    public TextMeshProUGUI consumableDescription;
    public TextMeshProUGUI consumableNameText;
    public TextMeshProUGUI consumableHPRecoversText;

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
        itemInformationPanel.SetActive(false);
        consumableInformationPanel.SetActive(false);
    }

    void Start()
    {
        closeButton.onClick.AddListener(CloseShop);
    }

    void PopulateItemShop(List<Item> itemsForSale)
    {
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in itemsForSale)
        {
            GameObject itemGO = Instantiate(itemPrefab, itemsParent);
            itemGO.GetComponent<ItemUI>().SetupItemForSale(item, this);
        }
    }

    void PopulateConsumableShop(List<ConsumableItem> itemsForSale)
    {
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (ConsumableItem consumableItem in itemsForSale)
        {
            GameObject itemGO = Instantiate(itemPrefab, itemsParent);
            itemGO.GetComponent<ItemUI>().SetupConsumableItemForSale(consumableItem, this);
        }
    }

    public void BuyItem(Item item)
    {
        if (item != null)
        {
            Debug.Log("Bought " + item.itemName);
        }
    }

    public void BuyConsumableItem(ConsumableItem consumableItem)
    {
        if (consumableItem != null)
        {
            Debug.Log("Bought " + consumableItem.itemName);
        }
    }

    public void CloseShop()
    {
        isOpenShop = false;
        shopPanel.SetActive(false);
        itemInformationPanel.SetActive(false);
        consumableInformationPanel.SetActive(false);
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
                PopulateItemShop(weaponItemsForSale);
                break;
            case ShopType.EquipmentShop:
                PopulateItemShop(equipmentItemsForSale);
                break;
            case ShopType.ConsumableShop:
                PopulateConsumableShop(consumableItemsForSale);
                break;
        }
    }

    public void DisplayItemInformation(Item item)
    {
        itemInformationPanel.SetActive(true);
        itemNameText.text = item.itemName;

        if (item is ArmorItem armorItem)
        {
            itemHPBonusText.text = "+" + armorItem.hpBonus.ToString();
            itemDefensePowerText.text = "+" + armorItem.defensePower.ToString();
            itemAttackPowerText.text = "-";
        }
        else if (item is WeaponItem weaponItem)
        {
            itemHPBonusText.text = "-";
            itemDefensePowerText.text = "-";
            itemAttackPowerText.text = "+" + weaponItem.attackPower.ToString();
        }
    }

    public void DisplayConsumableInformation(ConsumableItem consumableItem)
    {
        consumableInformationPanel.SetActive(true);
        consumableNameText.text = consumableItem.itemName;

        if (consumableItem is HealthPotion healthPotion)
        {
            if (healthPotion.potionType == HealthPotionType.Miraculous)
            {
                consumableHPRecoversText.text = "Full HP";
            }
            else
            {
                consumableHPRecoversText.text = "+" + healthPotion.healthRestoreAmount.ToString();
            }

            consumableDescription.text = healthPotion.description;
        }
    }
}