using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerGroundCheck playerGroundCheck;
    private PlayerAnimations playerAnimations;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerMovement = GetComponent<PlayerMovement>();
        playerGroundCheck = GetComponent<PlayerGroundCheck>();
        playerAnimations = GetComponentInChildren<PlayerAnimations>();
    }
    private void FixedUpdate()
    {
        playerMovement.HandleMovementDirection();

        if (playerGroundCheck.isGrounded)
            playerMovement.HandlePlayerMovement();

        if(playerMovement.movementInput.magnitude > 0.1f)
            playerMovement.HandlePlayerTurning(playerMovement.movementDir);
        playerGroundCheck.HandleGroundCheck();
        playerAnimations.HandleWalkingAnimations();
    }
}