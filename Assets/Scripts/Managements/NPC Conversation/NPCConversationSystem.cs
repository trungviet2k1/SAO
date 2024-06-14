using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCConversationSystem : MonoBehaviour
{
    public static NPCConversationSystem Instance;

    public GameObject conversationUI;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public Image npcAvatarImage;
    public Button nextButton;
    public Button exitButton;

    private DialogueSubject dialogueSubject;
    private IDialogueState currentState;
    private string[] dialogueLines;
    private int currentLineIndex = 0;

    private GuideNPC guideNPC;
    private string[] introDialogue;
    private string[] tipsDialogue;
    private Sprite npcAvatar;
    private string npcName;

    public bool IsInDialogue { get; private set; }

    private readonly DialogueInProgressState inProgressState = new();
    private readonly DialogueEndState endState = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        conversationUI.SetActive(false);
        nextButton.onClick.AddListener(ContinueDialogue);
        exitButton.onClick.AddListener(EndDialogue);
        dialogueSubject = new DialogueSubject();
    }

    void Update()
    {
        currentState?.UpdateState();
    }

    public void SetState(IDialogueState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState(this);
    }

    public void StartDialogue(string npcName, string[] lines, Sprite npcAvatar)
    {
        IsInDialogue = true;
        conversationUI.SetActive(true);
        this.npcName = npcName;
        npcNameText.text = npcName;
        this.npcAvatar = npcAvatar;
        npcAvatarImage.sprite = npcAvatar;
        dialogueLines = lines;
        currentLineIndex = 0;
        ShowCurrentLine();
        dialogueSubject.NotifyDialogueStart(new Dialogue(npcName, lines));

        SetState(inProgressState);
        nextButton.gameObject.SetActive(true);
    }

    public void ContinueDialogue()
    {
        ShowCurrentLine();
        currentLineIndex++;
    }

    public void ShowCurrentLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        IsInDialogue = false;
        conversationUI.SetActive(false);
        dialogueSubject.NotifyDialogueEnd();

        SetState(endState);
        nextButton.gameObject.SetActive(false);
    }

    public void AttachObserver(IDialogueObserver observer)
    {
        dialogueSubject.Attach(observer);
    }

    public void DetachObserver(IDialogueObserver observer)
    {
        dialogueSubject.Detach(observer);
    }

    public void LoadDialogue(string[] introDialogue, string[] tipsDialogue, Sprite npcAvatar)
    {
        this.introDialogue = introDialogue;
        this.tipsDialogue = tipsDialogue;
        this.npcAvatar = npcAvatar;
    }
}