using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<Item> inventoryItems = new List<Item>();
    public float currentBagWeight = 0;
    public float maxBagWeight = 300;

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
    }

    public void AddItem(Item item)
    {
        if (currentBagWeight + item.weight <= maxBagWeight)
        {
            inventoryItems.Add(item);
            currentBagWeight += item.weight;
            InventorySystem.Instance.UpdateInventory();
        }
        else
        {
            Debug.Log("Not enough weight capacity to add the item!");
        }
    }

    public void RemoveItem(Item item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
            currentBagWeight -= item.weight;
            InventorySystem.Instance.UpdateInventory();
        }
    }
}