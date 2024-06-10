using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    [Header("UI References")]
    public GameObject inventoryUI;
    public TextMeshProUGUI healthValue;
    public TextMeshProUGUI manaValue;
    public TextMeshProUGUI armorValue;
    public TextMeshProUGUI weightValue;
    public Transform itemSlotContainer;
    public List<ItemSlot> itemSlots;

    private InputAction inventoryAction;

    public bool IsInventoryOpen { get; private set; } = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
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

        if (IsInventoryOpen)
        {
            UpdateInventory();
        }
    }

    public void UpdateInventory()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < InventoryManager.Instance.inventoryItems.Count && i < itemSlots.Count; i++)
        {
            Item item = InventoryManager.Instance.inventoryItems[i];
            ItemSlot slot = itemSlots[i];

            GameObject itemObject = Instantiate(item.prefab, slot.transform);
            itemObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        weightValue.text = $"{InventoryManager.Instance.currentBagWeight} / {InventoryManager.Instance.maxBagWeight}";
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