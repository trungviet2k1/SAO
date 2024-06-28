using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    [Header("Inventory UI")]
    public GameObject inventoryUI;
    public TextMeshProUGUI levelValue;
    public TextMeshProUGUI expValue;
    public Slider expSlider;
    public TextMeshProUGUI healthValue;
    public TextMeshProUGUI manaValue;
    public TextMeshProUGUI armorValue;
    public TextMeshProUGUI currencyValue;
    public TextMeshProUGUI weightValue;

    [Header("Slot List")]
    public Transform itemSlotContainer;
    public List<ItemSlot> itemSlots;
    public List<EquipSlot> equipSlots;

    private InputAction inventoryAction;
    private InputAction addXPAction;
    private InventoryManager inventoryManager;
    private readonly Dictionary<Item, int> itemPositions = new();
    private readonly Dictionary<ConsumableItem, int> consumableItemPositions = new();

    public bool IsInventoryOpen { get; private set; } = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        inventoryUI.SetActive(false);
    }

    void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        inventoryAction = playerInput.actions["Inventory"];
        inventoryAction.performed += ToggleInventory;

        addXPAction = playerInput.actions["AddXP"];
        addXPAction.performed += AddXPTest;

        CharacterLevelSystem.Instance.OnLevelUp += UpdateLevelValue;
        CharacterLevelSystem.Instance.OnXPChanged += UpdateExpValue;
        HealthSystem.Instance.OnHealthChanged += UpdateHealthValue;

        UpdateHealthValue(HealthSystem.Instance.currentHealth);
        UpdateLevelValue();
        UpdateExpValue();

        inventoryManager = InventoryManager.Instance;
        inventoryManager.OnBagWeightChanged += UpdateWeightValue;

        CurrencyManager.Instance.OnMoneyChanged += UpdateCurrencyValue;
        UpdateCurrencyValue(CurrencyManager.Instance.CurrentMoney);
    }

    void OnDestroy()
    {
        if (inventoryAction != null)
        {
            inventoryAction.performed -= ToggleInventory;
        }

        if (CharacterLevelSystem.Instance != null)
        {
            CharacterLevelSystem.Instance.OnLevelUp -= UpdateLevelValue;
            CharacterLevelSystem.Instance.OnXPChanged -= UpdateExpValue;
        }

        HealthSystem.Instance.OnHealthChanged -= UpdateHealthValue;

        if (inventoryManager != null)
        {
            inventoryManager.OnBagWeightChanged -= UpdateWeightValue;
        }

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnMoneyChanged -= UpdateCurrencyValue;
        }
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        IsInventoryOpen = !IsInventoryOpen;
        inventoryUI.SetActive(IsInventoryOpen);
    }

    public void UpdateInventory()
    {
        UpdateWeightValue();
        UpdateItemSlot();
    }

    void UpdateWeightValue()
    {
        inventoryManager = InventoryManager.Instance;
        float currentWeight = inventoryManager.GetCurrentBagWeight();
        float maxWeight = inventoryManager.GetMaxBagWeight();
        weightValue.text = $"{currentWeight:F2} / {maxWeight}";
    }

    public void UpdateWeightValue(float newWeight)
    {
        weightValue.text = $"{newWeight:F2} / {inventoryManager.GetMaxBagWeight():F2}";
    }

    void UpdateItemSlot()
    {
        HashSet<int> usedSlots = new();
        itemPositions.Clear();
        consumableItemPositions.Clear();

        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].transform.childCount > 0)
            {
                var itemComponent = itemSlots[i].transform.GetChild(0).GetComponent<ItemComponent>();
                var consumableItemComponent = itemSlots[i].transform.GetChild(0).GetComponent<ConsumableItemComponent>();

                if (itemComponent != null)
                {
                    usedSlots.Add(i);
                    itemPositions[itemComponent.item] = i;
                }

                if (consumableItemComponent != null)
                {
                    usedSlots.Add(i);
                    consumableItemPositions[consumableItemComponent.consumableItem] = i;
                }
            }
        }

        PlaceItemsInSlots(inventoryManager.inventoryItems, itemPositions, usedSlots);
        PlaceConsumableItemsInSlots(inventoryManager.inventoryConsumableItems, consumableItemPositions, usedSlots);
    }

    private void PlaceItemsInSlots(List<Item> items, Dictionary<Item, int> itemPositions, HashSet<int> usedSlots)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            int slotIndex;

            if (itemPositions.ContainsKey(item))
            {
                slotIndex = itemPositions[item];
            }
            else
            {
                slotIndex = GetEmptySlotIndex(usedSlots);
                if (slotIndex == -1) continue;

                itemPositions[item] = slotIndex;
                usedSlots.Add(slotIndex);
            }

            Transform slotTransform = itemSlots[slotIndex].transform;
            if (slotTransform.childCount == 0)
            {
                GameObject itemObject = Instantiate(item.prefab, slotTransform);
                itemObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                var itemComponent = itemObject.AddComponent<ItemComponent>();
                itemComponent.item = item;
            }
        }
    }

    private void PlaceConsumableItemsInSlots(List<ConsumableItem> items, Dictionary<ConsumableItem, int> itemPositions, HashSet<int> usedSlots)
    {
        for (int i = 0; i < items.Count; i++)
        {
            ConsumableItem item = items[i];
            int slotIndex;

            if (itemPositions.ContainsKey(item))
            {
                slotIndex = itemPositions[item];
            }
            else
            {
                slotIndex = GetEmptySlotIndex(usedSlots);
                if (slotIndex == -1) continue;

                itemPositions[item] = slotIndex;
                usedSlots.Add(slotIndex);
            }

            Transform slotTransform = itemSlots[slotIndex].transform;
            if (slotTransform.childCount == 0)
            {
                GameObject itemObject = Instantiate(item.prefabIcon, slotTransform);
                itemObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                var itemComponent = itemObject.AddComponent<ConsumableItemComponent>();
                itemComponent.consumableItem = item;
                itemComponent.stackCountText = itemObject.GetComponentInChildren<TextMeshProUGUI>();
                itemComponent.UpdateStackCount();
            }
            else
            {
                var existingItemComponent = slotTransform.GetChild(0).GetComponent<ConsumableItemComponent>();
                if (existingItemComponent != null && existingItemComponent.consumableItem == item)
                {
                    existingItemComponent.UpdateStackCount();
                }
            }
        }
    }

    private int GetEmptySlotIndex(HashSet<int> usedSlots)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (!usedSlots.Contains(i) && itemSlots[i].transform.childCount == 0)
            {
                return i;
            }
        }
        return -1;
    }

    private void AddXPTest(InputAction.CallbackContext context)
    {
        CharacterLevelSystem.Instance.AddXPTest();
    }

    public void UpdateLevelValue()
    {
        levelValue.text = $"{CharacterLevelSystem.Instance.Level}";
    }

    public void UpdateExpValue()
    {
        float expPercentage = CharacterLevelSystem.Instance.GetXPPercentage();
        string expText = $"EXP: {expPercentage:F2}%";
        expValue.text = expText;
        expSlider.value = expPercentage / 100f;
    }

    public void UpdateHealthValue(float currentHealth)
    {
        healthValue.text = $"{currentHealth}";
    }

    public void UpdateManaValue(float currentMana)
    {
        manaValue.text = $"{currentMana}";
    }

    public void UpdateArmorValue(float currentArmor)
    {
        armorValue.text = $"{currentArmor}";
    }

    public void CloseInventory()
    {
        IsInventoryOpen = false;
        inventoryUI.SetActive(false);
    }

    private void UpdateCurrencyValue(int newAmount)
    {
        currencyValue.text = $"{newAmount}";
    }
}