using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLevelMusic : MonoBehaviour
{
    [SerializeField] private AudioSource levelMusicSource;

    private void Awake()
    {
        levelMusicSource.gameObject.SetActive(false);
    }
}