using TMPro;
using UnityEngine;

public class ConsumableInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI consumableNameText;
    public TextMeshProUGUI consumableDescription;
    public TextMeshProUGUI consumableHPRecoversText;

    public void Display(ConsumableItem consumableItem)
    {
        gameObject.SetActive(true);
        consumableNameText.text = consumableItem.itemName;
        consumableDescription.text = consumableItem.description;

        if (consumableItem is HealthPotion healthPotion)
        {
            consumableHPRecoversText.text = healthPotion.potionType == HealthPotionType.Miraculous ? "Full HP" : "+" + healthPotion.healthRestoreAmount;
        }
    }
}