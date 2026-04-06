using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using System;

public class CameraSensitivityManager : MonoBehaviour
{
    public static CameraSensitivityManager Instance { get; private set; }

    public const string X_SENS_KEY = "CameraXSens";
    public const string Y_SENS_KEY = "CameraYSens";

    private CinemachineFreeLook playerCamera;

    private static float xSensitivity;
    private static float ySensitivity;

    public float GetCurrentX() => xSensitivity;
    public float GetCurrentY() => ySensitivity;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;

        GetDataFromSettings();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(FindObjectOfType<SettingsMenu>() == null)
            RefreshReferencesAndApply();
    }

    public void RefreshReferencesAndApply()
    {
        playerCamera = FindAnyObjectByType<CinemachineFreeLook>();

        GetDataFromSettings();

        ApplyToCamera();
    }

    private void GetDataFromSettings()
    {
        xSensitivity = PlayerPrefs.GetFloat(X_SENS_KEY, 300f);
        ySensitivity = PlayerPrefs.GetFloat(Y_SENS_KEY, 5f);
    }

    private void ApplyToCamera()
    {
        if (playerCamera != null)
        {
            playerCamera.m_XAxis.m_MaxSpeed = xSensitivity;
            playerCamera.m_YAxis.m_MaxSpeed = ySensitivity;
        }
    }
}
