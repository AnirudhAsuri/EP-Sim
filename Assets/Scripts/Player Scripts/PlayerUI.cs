using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private GameObject deathScreen;
    private string deathScreenTag = "Death Screen";

    public Slider playerHealthSlider;
    private string healthSliderTag = "Health Slider";
    private PlayerHealth playerHealth;

    private void Awake()
    {
        if (deathScreen == null)
        {
            deathScreen = GameObject.FindGameObjectWithTag(deathScreenTag);
        }

        if (deathScreen == null)
        {
            Debug.Log("Null");
        }
    }

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        if(playerHealthSlider == null)
        {
            GameObject playerHealthSliderObject = GameObject.FindGameObjectWithTag(healthSliderTag);

            playerHealthSlider = playerHealthSliderObject.GetComponent<Slider>();
        }

        deathScreen.SetActive(false);
        playerHealthSlider.maxValue = playerHealth.totalHealth;
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