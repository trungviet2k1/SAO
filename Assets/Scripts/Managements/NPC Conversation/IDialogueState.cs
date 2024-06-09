public interface IDialogueState
{
    void EnterState(DialogueSystem dialogueSystem);
    void UpdateState();
    void ExitState();
}