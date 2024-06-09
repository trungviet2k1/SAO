using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    public GameObject dialogueUI;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public Button exitButton;

    private DialogueSubject dialogueSubject;
    private IDialogueState currentState;
    private string[] dialogueLines;
    private int currentLineIndex = 0;

    public bool IsInDialogue { get; private set; }

    private readonly DialogueInProgressState inProgressState = new();
    private readonly DialogueEndState endState = new();

    void Awake()
    {
        Instance = this;
        dialogueUI.SetActive(false);
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

    public void StartDialogue(string npcName, string[] lines)
    {
        IsInDialogue = true;
        dialogueUI.SetActive(true);
        npcNameText.text = npcName;
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
        dialogueUI.SetActive(false);
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
}