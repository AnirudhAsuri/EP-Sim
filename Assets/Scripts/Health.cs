using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public int totalHealth;

    public float currentHealth;

    public void InitialiseTotalHealth()
    {
        currentHealth = totalHealth;
    }

    public abstract void TakeDamage(float damage);
}