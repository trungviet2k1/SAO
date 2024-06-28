using TMPro;
using UnityEngine;

public class ItemInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemHPBonusText;
    public TextMeshProUGUI itemAttackPowerText;
    public TextMeshProUGUI itemDefensePowerText;

    public void Display(Item item)
    {
        gameObject.SetActive(true);
        itemNameText.text = item.itemName;

        if (item is ArmorItem armorItem)
        {
            itemHPBonusText.text = "+" + armorItem.hpBonus;
            itemDefensePowerText.text = "+" + armorItem.defensePower;
            itemAttackPowerText.text = "-";
        }
        else if (item is WeaponItem weaponItem)
        {
            itemHPBonusText.text = "-";
            itemDefensePowerText.text = "-";
            itemAttackPowerText.text = "+" + weaponItem.attackPower;
        }
    }
}