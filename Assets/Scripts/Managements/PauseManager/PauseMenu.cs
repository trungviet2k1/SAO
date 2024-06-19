using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    [Header("Controls UI")]
    public GameObject pauseMenuUI;
    public GameObject optionsPanelUI;

    [Header("Settings Panel UI")]
    public GameObject[] settingsPanels;

    [Header("Buttons Color")]
    public Button[] buttons;
    public Color selectedNormalColor = new Color32(0x86, 0x83, 0x83, 0xFF);
    public Color defaultNormalColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    public Color highlightedColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    public Color pressedColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    public Color selectedColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);

    [Header("Text Color")]
    public Color selectedTextColor = new Color32(0xFF, 0xE5, 0x00, 0xFF);
    public Color defaultTextColor = new Color32(0x00, 0x00, 0x00, 0xFF);

    [Header("Audio Settings UI")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private Button selectedButton;
    [HideInInspector] public bool gameIsPaused = false;
    private GameObject currentActivePanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        pauseMenuUI.SetActive(false);
        optionsPanelUI.SetActive(false);

        InitializeAudioSettings();
    }

    private void InitializeAudioSettings()
    {
        masterVolumeSlider.value = 1f;
        musicVolumeSlider.value = 1f;
        sfxVolumeSlider.value = 1f;

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        AudioManager.Instance.SetMasterVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameIsPaused)
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        SetCursorState(false);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        SetCursorState(true);
    }

    public void ToggleOptionPanel()
    {
        optionsPanelUI.SetActive(!optionsPanelUI.activeSelf);

        if (optionsPanelUI.activeSelf)
        {
            ShowPanel(settingsPanels[0]);
            SetSelectedButton(buttons[0]);
        }
    }

    public void ShowPanel(GameObject panelToShow)
    {
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
        }

        panelToShow.SetActive(true);
        currentActivePanel = panelToShow;
    }

    public void CloseOptionPanel()
    {
        optionsPanelUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void ShowAudioSettings()
    {
        ShowPanel(settingsPanels[0]);
        SetSelectedButton(buttons[0]);
    }

    public void ShowGraphicsSettings()
    {
        ShowPanel(settingsPanels[1]);
        SetSelectedButton(buttons[1]);
    }

    public void ShowControlsSettings()
    {
        ShowPanel(settingsPanels[2]);
        SetSelectedButton(buttons[2]);
    }

    public void ShowGameplaySettings()
    {
        ShowPanel(settingsPanels[3]);
        SetSelectedButton(buttons[3]);
    }

    public void ShowOthersSettings()
    {
        ShowPanel(settingsPanels[4]);
        SetSelectedButton(buttons[4]);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game Menu");
    }

    public void SetCursorState(bool isPaused)
    {
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }

    private void SetSelectedButton(Button button)
    {
        selectedButton = button;
        UpdateButtonColors();
    }

    private void UpdateButtonColors()
    {
        foreach (Button btn in buttons)
        {
            ColorBlock cb = btn.colors;
            cb.normalColor = btn == selectedButton ? selectedNormalColor : defaultNormalColor;
            cb.highlightedColor = highlightedColor;
            cb.pressedColor = pressedColor;
            cb.selectedColor = selectedColor;
            btn.colors = cb;

            TextMeshProUGUI buttonText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.color = btn == selectedButton ? selectedTextColor : defaultTextColor;
            }
        }
    }
}