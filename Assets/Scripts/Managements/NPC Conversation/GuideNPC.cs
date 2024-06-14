using UnityEngine;

public class GuideNPC : InteractableNPC
{
    public string[] introDialogue;
    public string[] tipsDialogue;

    public bool hasCompletedTutorial = false;

    public override void Start()
    {
        base.Start();
        npcName = "Kirito";
        npcAvatar = Resources.Load<Sprite>("Avatars/Kirito");
        NPCConversationSystem.Instance.LoadDialogue(introDialogue, tipsDialogue, npcAvatar);
    }

    protected override void Interaction()
    {
        base.Interaction();
        if (!hasCompletedTutorial)
        {
            StartTutorial();
        }
        else
        {
            StartTips();
        }
    }

    private void StartTutorial()
    {
        NPCConversationSystem.Instance.StartDialogue(npcName, introDialogue, npcAvatar);
        hasCompletedTutorial = true;
    }

    private void StartTips()
    {
        NPCConversationSystem.Instance.StartDialogue(npcName, tipsDialogue, npcAvatar);
    }
}