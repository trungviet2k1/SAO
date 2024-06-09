using System.Collections.Generic;

public class DialogueSubject
{
    private List<IDialogueObserver> observers = new List<IDialogueObserver>();

    public void Attach(IDialogueObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IDialogueObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyDialogueStart(Dialogue dialogue)
    {
        foreach (var observer in observers)
        {
            observer.OnDialogueStart(dialogue);
        }
    }

    public void NotifyDialogueEnd()
    {
        foreach (var observer in observers)
        {
            observer.OnDialogueEnd();
        }
    }
}