using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; set; }

    public GameObject shopPanel;
    public GameObject itemPrefab;
    public Transform itemsParent;
    public Button closeButton;

    public List<Item> itemsForSale;
    //private Item selectedItem;
    [HideInInspector] public bool isOpenShop = false;

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

        shopPanel.SetActive(false);
    }

    void Start()
    {
        closeButton.onClick.AddListener(CloseShop);
        PopulateShop();
    }

    void PopulateShop()
    {
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in itemsForSale)
        {
            GameObject itemGO = Instantiate(itemPrefab, itemsParent);
            itemGO.GetComponent<ItemUI>().Setup(item, this);
        }
    }

    public void BuyItem(Item item)
    {
        if (item != null)
        {
            Debug.Log("Bought " + item.itemName);
        }
    }

    public void CloseShop()
    {
        isOpenShop = false;
        shopPanel.SetActive(false);
    }

    public void OpenShop()
    {
        isOpenShop = true;
        shopPanel.SetActive(true);
    }
}