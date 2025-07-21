using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [SerializeField] private float totalStamina;
    public float currentStamina;

    [SerializeField] private float staminaDrainRate;
    [SerializeField] private float staminaRiseRate;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        currentStamina = totalStamina;
    }

    public void HandleStaminaChange()
    {
        if(playerMovement.isSprinting)
        {
            currentStamina -= staminaDrainRate;
        }
        else
        {
            currentStamina += staminaRiseRate;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, totalStamina);
    }
}