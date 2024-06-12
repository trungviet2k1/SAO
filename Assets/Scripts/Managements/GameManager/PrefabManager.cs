using UnityEngine;
using System.Collections.Generic;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance { get; set; }

    [SerializeField] private List<GameObject> weaponPrefabs;

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

    public GameObject GetWeaponPrefab(string itemName)
    {
        foreach (var prefab in weaponPrefabs)
        {
            if (prefab.name == itemName)
            {
                return prefab;
            }
        }

        Debug.LogWarning("Weapon prefab not found for: " + itemName);
        return null;
    }
}