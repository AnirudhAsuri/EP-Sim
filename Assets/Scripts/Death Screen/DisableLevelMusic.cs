using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLevelMusic : MonoBehaviour
{
    [SerializeField] private GameObject levelMusicSource;
    private string levelMusicTag = "Level Music Source";

    private void Awake()
    {
        levelMusicSource = GameObject.FindGameObjectWithTag(levelMusicTag);
        levelMusicSource.SetActive(false);
    }
}