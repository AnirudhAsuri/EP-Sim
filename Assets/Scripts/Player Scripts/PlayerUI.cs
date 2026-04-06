using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;

    public Slider playerHealthSlider;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        deathScreen.SetActive(false);
        playerHealthSlider.maxValue = playerHealth.totalHealth;
    }

    private void Update()
    {
        if(playerHealth.currentHealth <= 0f)
        {
            OpenDeathScreen();
        }    
    }

    public void OpenDeathScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        deathScreen.SetActive(true);
    }

    public void HandleHealthSlider()
    {
        if (playerHealthSlider != null)
            playerHealthSlider.value = playerHealth.currentHealth;
    }

    public void UpdateHealthSliderTotalHealth(float newHealth)
    {
        playerHealthSlider.maxValue = newHealth;
    }
}