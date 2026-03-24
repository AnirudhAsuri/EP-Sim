using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GongManager : MonoBehaviour
{
    [SerializeField] private AudioClip gongSoundClip;
    [SerializeField] private AudioSource menuMusicPlayer;

    private void OnTriggerEnter(Collider other)
    {
        menuMusicPlayer.Stop();
        Time.timeScale = 0;
        SoundFXManager.instance.PlaySoundEffect(gongSoundClip, transform, 1f);
    }
}