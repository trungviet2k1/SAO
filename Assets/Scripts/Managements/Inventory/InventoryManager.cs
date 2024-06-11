using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; set; }

    public List<Item> inventoryItems = new();
    [SerializeField] private float maxBagWeight = 300f;
    private BagWeightManager bagWeightManager;

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

        bagWeightManager = new BagWeightManager(maxBagWeight);
    }

    public void AddItem(Item item)
    {
        if (bagWeightManager.CanAddItem(item.weight))
        {
            inventoryItems.Add(item);
            bagWeightManager.AddItemWeight(item.weight);
            InventorySystem.Instance.UpdateInventory();
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

    public float GetCurrentBagWeight()
    {
        return bagWeightManager.CurrentBagWeight;
    }

    public float GetMaxBagWeight()
    {
        return bagWeightManager.MaxBagWeight;
    }
}