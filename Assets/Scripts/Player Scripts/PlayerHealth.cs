using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    private PlayerMovement playerMovement;

    public int damageTaken;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        InitialiseTotalHealth();
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;
        damageTaken = damage;
    }
}