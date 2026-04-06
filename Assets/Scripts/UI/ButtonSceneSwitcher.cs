using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSceneChanger : MonoBehaviour
{
    public LevelLoader levelLoader;

    private string levelPickerScene = "Level Picker Scene";
    private string settingsScene = "Settings Scene";
    private string mainMenuScene = "Main Menu Scene";

    private float time = 0f;
    private bool timerOn = false;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        EnableInteractability();

        levelLoader = FindAnyObjectByType<LevelLoader>();
    }

    private void Update()
    {
        if(timerOn)
        {
            time += Time.unscaledDeltaTime;
        }

        if (time >= 1.5f)
        {
            Time.timeScale = 1f;
            time = 0f;
            timerOn = false;
            levelLoader.LoadLevel(levelPickerScene);
        }
    }

    public void HandleMainMenuToLevelPicker()
    {
        timerOn = true;
        DisableInteractability();
    }

    public void HandleSwitchToLevelPicker()
    {
        Time.timeScale = 1f;
        DisableInteractability();
        levelLoader.LoadLevel(levelPickerScene);
    }

    public void HandleSwitchToSettings()
    {
        Time.timeScale = 1f;
        DisableInteractability();
        levelLoader.LoadLevel(settingsScene);
    }

    public void HandleGameExit()
    {
        Application.Quit();
    }

    public void HandleSwitchToMainMenu()
    {
        Time.timeScale = 1f;
        DisableInteractability();
        levelLoader.LoadLevel(mainMenuScene);
    }

    public void RestartLevel()
    {
        levelLoader.LoadLevel(SceneManager.GetActiveScene().name);
        DisableInteractability();
    }

    private void DisableInteractability()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void EnableInteractability()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}