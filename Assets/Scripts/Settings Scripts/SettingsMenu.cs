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
    private const string SFXKey = "SoundFXVolume";

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundFXSlider;

    [SerializeField] private Slider xSensSlider;
    [SerializeField] private Slider ySensSlider;

    public const string X_SENS_KEY = "CameraXSens";
    public const string Y_SENS_KEY = "CameraYSens";

    private string freezeFrameStatus = "Freeze Frame Status";

    private void Start()
    {
        LoadSensitivitySettings();

        LoadMusicSettings();
    }

    private void OnDestroy()
    {
        PlayerPrefs.Save();
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
        PlayerPrefs.SetFloat(X_SENS_KEY, sensitivity);
    }

    public void SetYSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat(Y_SENS_KEY, sensitivity);
    }

    private void LoadSensitivitySettings()
    {
        float savedXSens = PlayerPrefs.GetFloat(X_SENS_KEY, 300f);
        float savedYSens = PlayerPrefs.GetFloat(Y_SENS_KEY, 5f);

        SetXSensitivity(savedXSens);
        SetYSensitivity(savedYSens);

        if (xSensSlider != null) xSensSlider.value = savedXSens;
        if (ySensSlider != null) ySensSlider.value = savedYSens;
    }

    public void SetFreezeFrameStatus(float value)
    {
        int val = (int)value;

        PlayerPrefs.SetInt(freezeFrameStatus, val);
    }
}