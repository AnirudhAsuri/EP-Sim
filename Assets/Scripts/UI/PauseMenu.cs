using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject deathScreen;
    public bool isPaused = false;

    [SerializeField] private InputActionReference pauseInput;

    private void Start()
    {
        pauseInput.action.Enable();
    }

    private void OnEnable()
    {
        if (pauseMenuUI != null && !deathScreen.activeSelf)
        {
            pauseInput.action.started += HandlePauseInput;
        }
    }

    private void HandlePauseInput(InputAction.CallbackContext obj)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        isPaused = false;
    }
}
