using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    public static EquipmentSystem Instance { get; set; }

    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject weaponSheath;

    GameObject currentWeaponInHand;
    GameObject currentWeaponInSheath;

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

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (currentWeaponInHand)
        {
            Destroy(currentWeaponInHand);
        }

        if (currentWeaponInSheath)
        {
            Destroy(currentWeaponInSheath);
        }

        currentWeaponInSheath = Instantiate(weaponPrefab, weaponSheath.transform);
    }

    public void UnEquipWeapon()
    {
        if (currentWeaponInHand)
        {
            Destroy(currentWeaponInHand);
            currentWeaponInHand = null;
        }

        if (currentWeaponInSheath)
        {
            Destroy(currentWeaponInSheath);
            currentWeaponInSheath = null;
        }
    }

    private GameObject CloneWeapon(GameObject original, Transform parent)
    {
        if (original)
        {
            return Instantiate(original, parent);
        }
        return null;
    }

    public void DrawWeapon()
    {
        if (currentWeaponInSheath)
        {
            currentWeaponInHand = CloneWeapon(currentWeaponInSheath, weaponHolder.transform);
            Destroy(currentWeaponInSheath);
        }
    }

    public void SheathWeapon()
    {
        if (currentWeaponInHand)
        {
            currentWeaponInSheath = CloneWeapon(currentWeaponInHand, weaponSheath.transform);
            Destroy(currentWeaponInHand);
        }
    }

    public void StartDealDamage()
    {
        if (currentWeaponInHand)
        {
            currentWeaponInHand.GetComponentInChildren<DamageDealer>().StartDealDamage();
        }
    }

    public void EndDealDamage()
    {
        if (currentWeaponInHand)
        {
            currentWeaponInHand.GetComponentInChildren<DamageDealer>().EndDealDamage();
        }
    }
}