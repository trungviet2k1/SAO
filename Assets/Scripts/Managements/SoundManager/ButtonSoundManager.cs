using UnityEngine;

public class ButtonSoundManager : MonoBehaviour
{
    public AudioClip buttonClickClip;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickClip);
    }
}