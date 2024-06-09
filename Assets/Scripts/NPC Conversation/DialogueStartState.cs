public class DialogueStartState : IDialogueState
{
    public void EnterState(DialogueSystem dialogueSystem)
    {
        dialogueSystem.dialogueUI.SetActive(true);
    }

    public void UpdateState() { }

    public void ExitState() { }
}