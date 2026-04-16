using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLevelMusic : MonoBehaviour
{
    private GameObject levelMusicObject;
    private AudioSource levelMusicSource;
    private string levelMusicTag = "Level Music Source";

    private void OnEnable()
    {
        levelMusicObject = GameObject.FindGameObjectWithTag(levelMusicTag);
        levelMusicSource = levelMusicObject.GetComponent<AudioSource>();

        levelMusicSource.Stop();
    }
}