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
        inventoryAction.performed -= ToggleInventory;
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        IsInventoryOpen = !IsInventoryOpen;
        inventoryUI.SetActive(IsInventoryOpen);

        if (IsInventoryOpen)
        {
            UpdateHealthValue(HealthSystem.Instance.currentHealth);
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