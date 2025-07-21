using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private GameObject playerDeadBody;

    public CinemachineFreeLook thirdPersonCamera;

    private void Start()
    {
        thirdPersonCamera = FindObjectOfType<CinemachineFreeLook>();
    }

    public void SwitchBodies()
    {
        GameObject deadBody = Instantiate(playerDeadBody, transform.position, transform.rotation);

        thirdPersonCamera.Follow = deadBody.transform;
        thirdPersonCamera.LookAt = deadBody.transform;

        Destroy(gameObject);
    }
}