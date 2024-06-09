public class DialogueEndState : IDialogueState
{
    public void EnterState(DialogueSystem dialogueSystem)
    {
        dialogueSystem.dialogueUI.SetActive(false);
    }

    public void UpdateState() { }

    public void ExitState() { }
}