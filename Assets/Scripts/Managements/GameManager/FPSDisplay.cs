using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    private float deltaTime = 0.0f;
    private float fps;
    private int frameCount = 0;
    private float elapsed = 0.0f;
    private const float refreshRate = 1.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        frameCount++;
        elapsed += Time.unscaledDeltaTime;

        if (elapsed >= refreshRate)
        {
            fps = frameCount / elapsed;
            fpsText.text = string.Format("{0:0.} FPS", fps);

            frameCount = 0;
            elapsed = 0.0f;
        }
    }
}