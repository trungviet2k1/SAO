using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    [Header("UI References")]
    public GameObject inventoryUI;
    public TextMeshProUGUI healthValue;
    public TextMeshProUGUI manaValue;
    public TextMeshProUGUI armorValue;
    public TextMeshProUGUI weightValue;
    public Transform itemSlotContainer;
    public List<ItemSlot> itemSlots;
    public List<EquipSlot> equipSlots;

    private InputAction inventoryAction;

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

        UpdateHealthValue(HealthSystem.Instance.currentHealth);
    }

    void OnDestroy()
    {
        if (inventoryAction != null)
        {
            inventoryAction.performed -= ToggleInventory;
        }
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        IsInventoryOpen = !IsInventoryOpen;
        inventoryUI.SetActive(IsInventoryOpen);
    }

    public void UpdateInventory()
    {
        var inventoryManager = InventoryManager.Instance;
        weightValue.text = $"{inventoryManager.GetCurrentBagWeight()} / {inventoryManager.GetMaxBagWeight()}";

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
            GameObject itemObject = Instantiate(item.prefab, itemSlots[i].transform);
            itemObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            itemObject.AddComponent<ItemComponent>().item = item;
        }
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