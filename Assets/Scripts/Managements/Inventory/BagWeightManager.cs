using UnityEngine;

public class BagWeightManager
{
    public float CurrentBagWeight { get; private set; }
    public float MaxBagWeight { get; private set; }

    public BagWeightManager(float maxBagWeight)
    {
        MaxBagWeight = maxBagWeight;
    }

    public bool CanAddItem(float itemWeight)
    {
        return CurrentBagWeight + itemWeight <= MaxBagWeight;
    }

    public void AddItemWeight(float itemWeight)
    {
        if (CanAddItem(itemWeight))
        {
            CurrentBagWeight += itemWeight;
        }
        else
        {
            Debug.LogWarning("Not enough weight capacity to add the item!");
        }
    }

    public void RemoveItemWeight(float itemWeight)
    {
        CurrentBagWeight -= itemWeight;
        if (CurrentBagWeight < 0)
        {
            CurrentBagWeight = 0;
        }
    }
}