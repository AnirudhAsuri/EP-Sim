using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private string musicVolume = "MusicVolume";
    private string soundFXVolume = "SoundFXVolume";

    private void Start()
    {
        ChangeMusicVolume();
        ChangeSFXVolume();
    }

    private void ChangeMusicVolume()
    {
        audioMixer.SetFloat(musicVolume, PlayerPrefs.GetFloat(musicVolume, 0f));
    }

    private void ChangeSFXVolume()
    {
        audioMixer.SetFloat(soundFXVolume, PlayerPrefs.GetFloat(soundFXVolume, 0f));
    }
}