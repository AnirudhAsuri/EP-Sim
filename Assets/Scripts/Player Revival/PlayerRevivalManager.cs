using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerRevivalManager : MonoBehaviour
{
    [SerializeField] private GameObject alivePlayer;
    private CinemachineFreeLook thirdPersonCamera;

    private string levelMusicTag = "Level Music Source";
    private GameObject levelMusicObject;
    private AudioSource levelMusicSource;

    [SerializeField] private float revivedInvulnerabilityDuration;

    private void Start()
    {
        levelMusicObject = GameObject.FindGameObjectWithTag(levelMusicTag);
        levelMusicSource = levelMusicObject.GetComponent<AudioSource>();
        thirdPersonCamera = FindAnyObjectByType<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        MyAdsManager.OnRewardedAdGranted += RevivePlayer;
    }

    private void OnDisable()
    {
        MyAdsManager.OnRewardedAdGranted -= RevivePlayer;
    }

    public void RevivePlayer()
    {
        if(RevivalState.Instance != null)
        {
            RevivalState.Instance.hasRevived = true;
        }

        var revivedPlayer = Instantiate(alivePlayer, transform.position, Quaternion.identity);

        thirdPersonCamera.Follow = revivedPlayer.transform;
        thirdPersonCamera.LookAt = revivedPlayer.transform;

        PowerUpEffects effects = revivedPlayer.GetComponentInChildren<PowerUpEffects>();

        effects.SetCameraReference(thirdPersonCamera);
        effects.ActivateInvulnerability(revivedInvulnerabilityDuration);

        levelMusicObject.SetActive(true);
        levelMusicSource.Play();

        Destroy(gameObject);
    }
}