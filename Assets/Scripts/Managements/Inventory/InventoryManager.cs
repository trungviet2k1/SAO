using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; set; }

    public List<Item> inventoryItems = new();
    public List<ConsumableItem> inventoryConsumableItems = new();

    [Header("Bag information")]
    public float currentBagWeight;
    public float maxBagWeight;

    private InventorySettings inventorySettings;
    private BagWeightManager bagWeightManager;

    public event Action<float> OnBagWeightChanged;

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

        inventorySettings = Resources.Load<InventorySettings>("InventorySettings");
        bagWeightManager = new BagWeightManager(inventorySettings.maxBagWeight);
        bagWeightManager.OnWeightChanged += HandleWeightChanged;
    }

    void Start()
    {
        currentBagWeight = GetCurrentBagWeight();
        maxBagWeight = GetMaxBagWeight();
    }

    void HandleWeightChanged(float newWeight)
    {
        currentBagWeight = newWeight;
        OnBagWeightChanged?.Invoke(newWeight);
    }

    public bool CanAddItem(Item item)
    {
        return inventoryItems.Count < inventorySettings.maxSlots || bagWeightManager.CanAddItem(item.weight);
    }

    public bool CanAddItem(ConsumableItem item)
    {
        return inventoryConsumableItems.Count < inventorySettings.maxSlots || bagWeightManager.CanAddItem(item.weight);
    }

    public bool AddNewItem(Item item)
    {
        if (CanAddItem(item))
        {
            Item newItem = Instantiate(item);
            inventoryItems.Add(newItem);
            bagWeightManager.AddItemWeight(item.weight);
            InventorySystem.Instance.UpdateInventory();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AddConsumableItem(ConsumableItem item)
    {
        if (CanAddItem(item))
        {
            if (item.stackable)
            {
                ConsumableItem existingItem = inventoryConsumableItems.Find(i => i.itemID == item.itemID && i.stackCount < i.maxStackCount);

                if (existingItem != null && existingItem.stackCount < existingItem.maxStackCount)
                {
                    existingItem.stackCount++;
                }
                else
                {
                    ConsumableItem newStack = Instantiate(item);
                    newStack.stackCount = 1;
                    inventoryConsumableItems.Add(newStack);
                }
            }
            else
            {
                inventoryConsumableItems.Add(item);
            }

            bagWeightManager.AddItemWeight(item.weight);
            InventorySystem.Instance.UpdateInventory();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ReturnItems(Item item)
    {
        if (CanAddItem(item))
        {
            inventoryItems.Add(item);
            bagWeightManager.AddItemWeight(item.weight);
            InventorySystem.Instance.UpdateInventory();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(Item item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
            bagWeightManager.RemoveItemWeight(item.weight);
            InventorySystem.Instance.UpdateInventory();
        }
    }

    public void RemoveItem(ConsumableItem item)
    {
        if (inventoryConsumableItems.Contains(item))
        {
            inventoryConsumableItems.Remove(item);
            bagWeightManager.RemoveItemWeight(item.weight);
            InventorySystem.Instance.UpdateInventory();
        }
    }

    public float GetCurrentBagWeight()
    {
        return bagWeightManager.CurrentBagWeight;
    }

    public float GetMaxBagWeight()
    {
        return inventorySettings.maxBagWeight;
    }
}