public class DialogueStartState : IDialogueState
{
    public void EnterState(NPCConversationSystem dialogueSystem)
    {
        dialogueSystem.conversationUI.SetActive(true);
    }

    public void UpdateState() { }

    public void ExitState() { }
}