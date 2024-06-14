using UnityEngine;

public class DialogueInProgressState : IDialogueState
{
    private NPCConversationSystem dialogueSystem;

    public void EnterState(NPCConversationSystem dialogueSystem)
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