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

    public void SwitchBodies(Vector3 pushDirection, float force)
    {
        GameObject deadBody = Instantiate(playerDeadBody, transform.position, transform.rotation);

        thirdPersonCamera.Follow = deadBody.transform;
        thirdPersonCamera.LookAt = deadBody.transform;

        Rigidbody deadBodyRigidbody = deadBody.GetComponent<Rigidbody>();

        if(deadBodyRigidbody != null)
        {
            deadBodyRigidbody.AddForce(pushDirection * force, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }
}