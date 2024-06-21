using DialogueEditor;
using UnityEngine;

public class GuideNPC : InteractableNPC
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
}