using UnityEngine;

public class DialogueInProgressState : IDialogueState
{
    private DialogueSystem dialogueSystem;

    public void EnterState(DialogueSystem dialogueSystem)
    {
        this.dialogueSystem = dialogueSystem;
    }

    public void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dialogueSystem.ContinueDialogue();
        }
    }

    public void ExitState() { }
}