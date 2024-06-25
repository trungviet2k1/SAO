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
        Debug.Log("Mở cửa hàng!");
        ShopManager.Instance.OpenShop();
        TriggerIdle();
    }
}