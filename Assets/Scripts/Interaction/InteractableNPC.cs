using UnityEngine;

public abstract class InteractableNPC : Interactable
{
    protected Animator animator;
    public string npcName;
    public Sprite npcAvatar;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interaction();
        }
    }

    protected override void Interaction()
    {
        base.Interaction();
        if (NPCConversationSystem.Instance.IsInDialogue) return;
        animator.SetTrigger("Wave");
    }
}