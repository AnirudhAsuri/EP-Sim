using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;

    public Button[] buttons;
    public GameObject levelButtons;

    private string bestTimeText = "Best Time";

    private void Awake()
    {
        if (levelButtons != null)
        {
            ButtonsToArray();

            int unlockedLevelCount = PlayerPrefs.GetInt("Unlocked Level", 1);

            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < unlockedLevelCount)
                {
                    buttons[i].interactable = true;

                    Transform timerTransform = buttons[i].transform.Find(bestTimeText);

                    if(timerTransform != null)
                    {
                        TextMeshProUGUI timeDisplay = timerTransform.GetComponent<TextMeshProUGUI>();

                        int displayLevelNum = i + 1;
                        float bestTime = PlayerPrefs.GetFloat("Level " + displayLevelNum + " Best Time", 999999f);

                        if(bestTime >= 0 && bestTime < 999999f)
                        {
                            timeDisplay.text = FormatTime(bestTime);
                        }

                        else
                        {
                            timeDisplay.text = "--:--";
                        }
                    }
                }
                else
                {
                    buttons[i].interactable = false;
                }
            }
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void HandleLevelOpening(int levelID)
    {
        string levelName = "Level " + levelID;
        levelLoader.LoadLevel(levelName);
    }

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