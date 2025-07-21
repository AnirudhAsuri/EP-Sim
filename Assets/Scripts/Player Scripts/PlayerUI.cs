using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider playerHealthSlider;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        playerHealthSlider.maxValue = playerHealth.totalHealth;
    }

    public void HandleHealthSlider()
    {
        if (playerHealthSlider != null)
            playerHealthSlider.value = playerHealth.currentHealth;
    }
}