using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonSceneChanger : MonoBehaviour
{
    private string levelPickerScene = "Level Picker Scene";
    private string settingsScene = "Settings Scene";
    
    public void HandleSwitchToLevelPicker()
    {
        SceneManager.LoadScene(levelPickerScene);
    }

    public void HandleSwitchToSettings()
    {
        SceneManager.LoadScene(settingsScene);
    }

    public void HandleGameExit()
    {
        Application.Quit();
    }
}