using System;
using UnityEngine;

public class CharacterLevelSystem : MonoBehaviour
{
    public static CharacterLevelSystem Instance { get; private set; }

    public int Level { get; private set; } = 1;
    public int CurrentXP { get; private set; } = 0;
    public int XPNeeded { get; private set; } = 100;

    public event Action OnLevelUp;
    public event Action OnXPChanged;

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
    }

    public void AddXP(int amount)
    {
        CurrentXP += amount;
        OnXPChanged?.Invoke();

        while (CurrentXP >= XPNeeded)
        {
            CurrentXP -= XPNeeded;
            LevelUp();
        }
    }

    public void AddXPTest()  // Method to add 10 XP for testing
    {
        AddXP(10);
    }

    private void LevelUp()
    {
        Level++;
        XPNeeded = CalculateXPNeeded(Level);
        OnLevelUp?.Invoke();
        OnXPChanged?.Invoke();
    }

    private int CalculateXPNeeded(int level)
    {
        return Mathf.FloorToInt(100 * Mathf.Pow(1.1f, level - 1));
    }

    public float GetXPPercentage()
    {
        return (float)CurrentXP / XPNeeded * 100f;
    }

    public void ResetLevel()
    {
        Level = 1;
        CurrentXP = 0;
        XPNeeded = 100;
        OnXPChanged?.Invoke();
    }
}