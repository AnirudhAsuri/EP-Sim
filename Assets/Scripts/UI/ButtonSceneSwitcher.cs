using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSceneChanger : MonoBehaviour
{
    private string levelPickerScene = "Level Picker Scene";
    private string settingsScene = "Settings Scene";
    private string mainMenuScene = "Main Menu Scene";

    public void HandleSwitchToLevelPicker()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelPickerScene);
    }

    public void HandleSwitchToSettings()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(settingsScene);
    }

    public void HandleGameExit()
    {
        Application.Quit();
    }

    public void HandleSwitchToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}