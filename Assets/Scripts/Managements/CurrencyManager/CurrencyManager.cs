using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public event Action<int> OnMoneyChanged;

    public int currentMoney;

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
    }

    public int CurrentMoney
    {
        get => currentMoney;
        private set
        {
            currentMoney = value;
            OnMoneyChanged?.Invoke(currentMoney);
        }
    }

    public void AddMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add a negative amount of money.");
            return;
        }
        CurrentMoney += amount;
    }

    public void SubtractMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot subtract a negative amount of money.");
            return;
        }

        if (currentMoney >= amount)
        {
            CurrentMoney -= amount;
        }
        else
        {
            Debug.LogWarning("Not enough money to subtract the requested amount.");
        }
    }

    public bool CanAfford(int amount)
    {
        return currentMoney >= amount;
    }
}