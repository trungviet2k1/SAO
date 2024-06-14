public interface IDialogueState
{
    void EnterState(NPCConversationSystem dialogueSystem);
    void UpdateState();
    void ExitState();
}