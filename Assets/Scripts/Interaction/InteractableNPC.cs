using UnityEngine;

public class InteractableNPC : Interactable
{
    private Animator animator;
    public string[] dialogueLines;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Interaction()
    {
        base.Interaction();
        animator.SetTrigger("Wave");
        DialogueSystem.Instance.StartDialogue(interactableName, dialogueLines);
    }
}