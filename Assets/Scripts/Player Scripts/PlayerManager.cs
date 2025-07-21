using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerGroundCheck playerGroundCheck;
    private PlayerAnimations playerAnimations;
    private PlayerHealth playerHealth;
    private PlayerDeath playerDeath;
    private PlayerStamina playerStamina;
    private PlayerUI playerUI;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerMovement = GetComponent<PlayerMovement>();
        playerGroundCheck = GetComponent<PlayerGroundCheck>();
        playerAnimations = GetComponentInChildren<PlayerAnimations>();
        playerHealth = GetComponent<PlayerHealth>();
        playerDeath = GetComponent<PlayerDeath>();
        playerStamina = GetComponent<PlayerStamina>();
        playerUI = GetComponent<PlayerUI>();
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

    private void Update()
    {
        if(playerHealth.currentHealth <= 0)
        {
            playerUI.playerHealthSlider.enabled = false;
            playerDeath.SwitchBodies();
        }

        playerUI.HandleHealthSlider();
    }
}