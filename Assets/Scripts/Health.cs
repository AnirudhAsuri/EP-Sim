using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public int totalHealth;

    public int currentHealth;

    public void InitialiseTotalHealth()
    {
        currentHealth = totalHealth;
    }

    public abstract void TakeDamage(int damage);
}