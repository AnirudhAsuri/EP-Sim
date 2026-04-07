using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerGroundCheck playerGroundCheck;
    private PlayerAnimations playerAnimations;
    private PowerUpEffects powerUpEffects;
    private PlayerHealth playerHealth;
    private PlayerDeath playerDeath;
    private PlayerUI playerUI;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        playerMovement = GetComponent<PlayerMovement>();
        playerGroundCheck = GetComponent<PlayerGroundCheck>();
        playerAnimations = GetComponentInChildren<PlayerAnimations>();
        powerUpEffects = GetComponentInChildren<PowerUpEffects>();
        playerHealth = GetComponent<PlayerHealth>();
        playerDeath = GetComponent<PlayerDeath>();
        playerUI = GetComponent<PlayerUI>();
    }
    private void FixedUpdate()
    {
        playerMovement.HandleMovementDirection();

        if (playerGroundCheck.isGrounded)
            playerMovement.HandlePlayerMovement();

        playerMovement.HandlePlayerMovementSpeed();
        playerMovement.HandleSprintDust();

        if(playerMovement.movementInput.magnitude > 0.1f)
            playerMovement.HandlePlayerTurning(playerMovement.movementDir);
        playerGroundCheck.HandleGroundCheck();
        playerAnimations.HandleWalkingAnimations();

        if(powerUpEffects != null)
        {
            playerAnimations.HandleGiantAnimations(powerUpEffects.isGiant);
        }
    }

    private void Update()
    {
        if (playerHealth == null)
            return;

        if(playerHealth.currentHealth <= 0)
        {
            playerUI.playerHealthSlider.enabled = false;

            Vector3 finalPushDirection = playerHealth.pushBackDirection;
            float finalPushForce = playerHealth.pushBackForce;
            
            playerDeath.SwitchBodies(finalPushDirection, finalPushForce);
        }

        playerUI.HandleHealthSlider();
    }
}