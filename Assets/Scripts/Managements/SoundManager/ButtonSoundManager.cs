using UnityEngine;

public class ButtonSoundManager : MonoBehaviour
{
    public static ButtonSoundManager Instance { get; private set; }

    public AudioClip buttonClickClip;
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickClip);
    }
}