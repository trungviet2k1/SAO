using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    [Header("Confirm Dialogue UI")]
    public GameObject confirmationDialogUI;
    public TextMeshProUGUI confirmationDialogText;
    public Button confirmButton;
    public Button cancelButton;

    [Header("Inventory UI")]
    public GameObject inventoryUI;
    public TextMeshProUGUI levelValue;
    public TextMeshProUGUI expValue;
    public Slider expSlider;
    public TextMeshProUGUI healthValue;
    public TextMeshProUGUI manaValue;
    public TextMeshProUGUI armorValue;
    public TextMeshProUGUI weightValue;
    

    [Header("Slot List")]
    public Transform itemSlotContainer;
    public List<ItemSlot> itemSlots;
    public List<EquipSlot> equipSlots;

    private InputAction inventoryAction;
    private InputAction addXPAction;
    private readonly Dictionary<Item, int> itemPositions = new();

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
        confirmationDialogUI.SetActive(false);
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

        confirmButton.onClick.AddListener(OnConfirmDelete);
        cancelButton.onClick.AddListener(OnCancelDelete);

        UpdateHealthValue(HealthSystem.Instance.currentHealth);
        UpdateLevelValue();
        UpdateExpValue();
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
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        IsInventoryOpen = !IsInventoryOpen;
        inventoryUI.SetActive(IsInventoryOpen);
    }

    private void AddXPTest(InputAction.CallbackContext context)
    {
        CharacterLevelSystem.Instance.AddXPTest();
    }

    public void ShowConfirmationDialog()
    {
        confirmationDialogText.text = "Are you sure you want to delete this item?";
        confirmationDialogUI.SetActive(true);
    }

    private void OnConfirmDelete()
    {
        /*if (itemSlotToDelete != null && itemSlotToDelete.Item != null)
        {
            InventoryManager.Instance.RemoveItem(item);
            itemSlotToDelete.Item = null;
            confirmationDialogUI.SetActive(false);
            itemSlotToDelete = null;
            UpdateInventory();
        }*/
    }

    private void OnCancelDelete()
    {
        /*if (itemSlotToDelete != null)
        {
            itemSlotToDelete.transform.position = itemSlotToDelete.originalPosition;
            itemSlotToDelete = null;
        }
        confirmationDialogUI.SetActive(false);*/
    }

    public void UpdateInventory()
    {
        var inventoryManager = InventoryManager.Instance;
        weightValue.text = $"{inventoryManager.GetCurrentBagWeight()} / {inventoryManager.GetMaxBagWeight()}";

        itemPositions.Clear();
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].Item != null)
            {
                Item item = itemSlots[i].Item.GetComponent<ItemComponent>().item;
                itemPositions[item] = i;
            }
        }

        foreach (Transform child in itemSlotContainer)
        {
            if (child.childCount > 0)
            {
                Destroy(child.GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < inventoryManager.inventoryItems.Count; i++)
        {
            Item item = inventoryManager.inventoryItems[i];
            int slotIndex = itemPositions.ContainsKey(item) ? itemPositions[item] : i;
            GameObject itemObject = Instantiate(item.prefab, itemSlots[slotIndex].transform);
            itemObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            itemObject.AddComponent<ItemComponent>().item = item;
        }
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
}