using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class WinManager : MonoBehaviour
{
    [SerializeField] private int levelNumber;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject blackCrossfade;

    [SerializeField] private GameObject playerUICanvas;
    [SerializeField] private GameObject enemyCountCanvas;

    public TextMeshProUGUI timeText;
    private float timer;
    private float slowMoTime = 3f;

    private PostProcessVolume postProcessVolume;
    private Vignette vignette;
    private Grain grain;

    private void Awake()
    {
        postProcessVolume = FindAnyObjectByType<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out grain);
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    private void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        timeText.text = formattedTime;
    }

    private void DisableUIElements()
    {
        playerUICanvas.SetActive(false);
        enemyCountCanvas.SetActive(false);
    }

    public void EnableWinScreen()
    {
        DisableUIElements();

        UpdateTimeUI();
        UpdatePlayerPrefs();

        StartCoroutine(WinScreenRoutine());
    }

    private IEnumerator WinScreenRoutine()
    {
        if (vignette != null)
        {
            vignette.enabled.value = true;
        }

        if (grain != null)
        {
            grain.enabled.value = true;
        }

        Time.timeScale = 0.25f;

        yield return new WaitForSecondsRealtime(slowMoTime);

        blackCrossfade.SetActive(true);

        winScreen.SetActive(true);

        Time.timeScale = 1f;
    }

    private void UpdatePlayerPrefs()
    {
        int reachedIndex = PlayerPrefs.GetInt("Reached Index", 1);

        if(levelNumber >= reachedIndex)
        {
            PlayerPrefs.SetInt("Reached Index", levelNumber + 1);
            PlayerPrefs.SetInt("Unlocked Level", levelNumber + 1);
        }

        string timeKey = "Level " + levelNumber + " Best Time";

        float previousBest = PlayerPrefs.GetFloat(timeKey, 999999f);

        if(timer < previousBest)
        {
            PlayerPrefs.SetFloat(timeKey, timer);
        }

        PlayerPrefs.Save();
    }
}