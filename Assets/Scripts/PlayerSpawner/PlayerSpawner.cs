using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    private CinemachineFreeLook thirdPersonCamera;

    private void Awake()
    {
        thirdPersonCamera = FindAnyObjectByType<CinemachineFreeLook>();
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        var player = Instantiate(playerObject, transform.position, transform.rotation);

        thirdPersonCamera.Follow = player.transform;
        thirdPersonCamera.LookAt = player.transform;

        PowerUpEffects effects = player.GetComponentInChildren<PowerUpEffects>();
        effects.SetCameraReference(thirdPersonCamera);
    }
}