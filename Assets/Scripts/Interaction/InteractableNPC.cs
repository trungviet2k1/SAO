﻿using DialogueEditor;
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
        if (ConversationManager.Instance.IsInDialogue) return;
        animator.SetTrigger("Wave");
    }

    public void TriggerWave()
    {
        if (animator != null)
        {
            animator.SetTrigger("Wave");
        }
    }

    public void TriggerIdle()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Wave");
            animator.SetTrigger("Idle");
        }
    }
}