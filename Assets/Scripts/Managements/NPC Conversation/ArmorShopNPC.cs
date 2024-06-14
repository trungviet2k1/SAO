using UnityEngine;

public class ArmorShopNPC : InteractableNPC
{
    public string[] startConversation;
    public string[] buyArmorDialogue;
    public string[] upgradeArmorDialogue;
    public string[] questDialogue;

    public override void Start()
    {
        base.Start();
        npcName = "Asuna";
        npcAvatar = Resources.Load<Sprite>("Avatars/Asuna");
    }

    protected override void Interaction()
    {
        base.Interaction();
        int choice = ShowInteractionOptions();
        switch (choice)
        {
            case 1:
                NPCConversationSystem.Instance.StartDialogue(npcName, startConversation, npcAvatar);
                break;
            case 2:
                NPCConversationSystem.Instance.StartDialogue(npcName, buyArmorDialogue, npcAvatar);
                break;
            case 3:
                NPCConversationSystem.Instance.StartDialogue(npcName, upgradeArmorDialogue, npcAvatar);
                break;
            case 4:
                NPCConversationSystem.Instance.StartDialogue(npcName, questDialogue, npcAvatar);
                break;
        }
    }

    private int ShowInteractionOptions()
    {
        return 1;
    }
}