using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private string musicVolume = "MusicVolume";
    private string soundFXVolume = "SoundFXVolume";

    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundFXSlider;

    [SerializeField] private Slider xSensSlider;
    [SerializeField] private Slider ySensSlider;

    public float cameraXSensitivity;
    public float cameraYSensitivity;

    private void Start()
    {
        if(CameraSensitivityManager.Instance != null)
        {
            xSensSlider.value = CameraSensitivityManager.Instance.GetCurrentX();
            ySensSlider.value = CameraSensitivityManager.Instance.GetCurrentY();
        }

        LoadMusicSettings();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(musicVolume, volume);

        PlayerPrefs.SetFloat(MusicKey, volume);
    }

    public void SetSoundFXVolume(float volume)
    {
        audioMixer.SetFloat(soundFXVolume, volume);

        PlayerPrefs.SetFloat(SFXKey, volume);
    }

    private void LoadMusicSettings()
    {
        float savedMusic = PlayerPrefs.GetFloat(MusicKey, 0f);
        float savedSFX = PlayerPrefs.GetFloat(SFXKey, 0f);

        SetMusicVolume(savedMusic);
        SetSoundFXVolume(savedSFX);

        if (musicSlider != null) musicSlider.value = savedMusic;
        if (soundFXSlider != null) soundFXSlider.value = savedSFX;
    }

    public void SetXSensitivity(float sensitivity)
    {
        cameraXSensitivity = sensitivity;

        if(CameraSensitivityManager.Instance != null)
        {
            CameraSensitivityManager.Instance.RefreshReferencesAndApply();
        }
    }

    public void SetYSensitivity(float sensitivity)
    {
        cameraYSensitivity = sensitivity;

        if (CameraSensitivityManager.Instance != null)
        {
            CameraSensitivityManager.Instance.RefreshReferencesAndApply();
        }
    }
}