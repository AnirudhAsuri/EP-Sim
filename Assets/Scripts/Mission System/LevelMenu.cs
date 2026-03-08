using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;
    public GameObject levelButtons;

    private void Awake()
    {
        if(levelButtons != null)
        {
            ButtonsToArray();
            int unlockedLevel = PlayerPrefs.GetInt("Unlocked Level", 5);

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = false;
            }
            for (int i = 0; i <= 4; i++)
            {
                buttons[i].interactable = true;
            }
        }
    }

    public void HandleLevelOpening(int levelID)
    {
        string levelName = "Level " + levelID;
        SceneManager.LoadScene(levelName);
    }

    /*public void HandleLevelUnlocking()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("Reached Index"))
        {
            PlayerPrefs.SetInt("Reached Index", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("Unlocked Level", PlayerPrefs.GetInt("Unlocked Level", 1) + 1);
            PlayerPrefs.Save();
        }
    }*/

    private void ButtonsToArray()
    {
        int childCount = levelButtons.transform.childCount;
        buttons = new Button[childCount];

        for(int i = 0; i < childCount; i++)
        {
            buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }
}