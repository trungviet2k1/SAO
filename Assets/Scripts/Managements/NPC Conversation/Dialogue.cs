public class Dialogue
{
    public string NPCName { get; set; }
    public string[] Lines { get; set; }

    public Dialogue(string npcName, string[] lines)
    {
        NPCName = npcName;
        Lines = lines;
    }
}