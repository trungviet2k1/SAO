using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    private CharacterLevelSystem levelSystem;

    void Start()
    {
        levelSystem = CharacterLevelSystem.Instance;
        levelSystem.OnLevelUp += UpdateUI;
        UpdateUI();
    }

    void OnDestroy()
    {
        if (levelSystem != null)
        {
            levelSystem.OnLevelUp -= UpdateUI;
        }
    }

    private void UpdateUI()
    {
        levelText.text = "Lv. " + levelSystem.Level;
    }
}