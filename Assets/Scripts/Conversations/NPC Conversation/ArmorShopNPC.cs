using DialogueEditor;
using UnityEngine;

public class ArmorShopNPC : InteractableNPC
{
    [SerializeField] private NPCConversation npcConversation;

    protected override void Interaction()
    {
        base.Interaction();
        if (!ConversationManager.Instance.IsInDialogue)
        {
            StartTutorial();
        }
    }

    private void StartTutorial()
    {
        ConversationManager.Instance.StartConversation(this, npcConversation);
    }

    public void OpenShop()
    {
        string shopName = $"{npcName}'s Shop";
        ShopManager.Instance.OpenShop(ShopType.EquipmentShop, shopName);
        TriggerIdle();
    }
}