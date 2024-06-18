using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame(int sceneIndex)
    {
        PlayerPrefs.SetInt("SceneToLoad", sceneIndex);
        SceneManager.LoadScene("LoadingScreen");
    }

    public void LoadGame()
    {
        Debug.Log("Load Game");
    }

    public void OpenSettings()
    {
        Debug.Log("Open Settings");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}