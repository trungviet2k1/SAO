public class DialogueEndState : IDialogueState
{
    public void EnterState(NPCConversationSystem dialogueSystem)
    {
        dialogueSystem.conversationUI.SetActive(false);
    }

    public void UpdateState() { }

    public void ExitState() { }
}