using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar;
    public TextMeshProUGUI progressText;

    void Start()
    {
        StartCoroutine(LoadAsynchronously(PlayerPrefs.GetInt("SceneToLoad")));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float progress = 0f;

        while (progress < 0.9f)
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            progressText.text = (progress * 100f).ToString("F0") + "%";
            yield return null;
        }

        float fakeLoadTime = 0f;
        while (fakeLoadTime < 5f)
        {
            fakeLoadTime += Time.deltaTime;
            float fakeProgress = Mathf.Lerp(0.9f, 1f, fakeLoadTime / 5f);
            progressBar.value = fakeProgress;
            progressText.text = (fakeProgress * 100f).ToString("F0") + "%";
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}