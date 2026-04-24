using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerHealth : Health
{
    public static PlayerHealth Instance;

    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;
    private Rigidbody playerRigidbody;
    private PowerUpEffects powerUpEffects;

    public float defaultHealth;

    public float damageTaken;
    public Vector3 pushBackDirection;
    public float pushBackForce;

    private float screenShakeForce;

    [SerializeField] private AudioClip playerHitAudioClip;
    [SerializeField] private AudioClip invulnHitAudioClip;

    private void Awake()
    {
        Instance = this;

        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        playerRigidbody = GetComponent<Rigidbody>();
        powerUpEffects = GetComponentInChildren<PowerUpEffects>();

        InitialiseTotalHealth();
    }

    private void Start()
    {
        defaultHealth = totalHealth;
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0f, 100f);
    }

    public override void TakeDamage(float damage, Vector3 direction, float force)
    {
        if (powerUpEffects.isChanging)
            return;

        if(!powerUpEffects.isInvulnerable)
        {
            screenShakeForce = damage * 0.01f;
            currentHealth -= damage;

            pushBackDirection = direction;
            pushBackForce = force;
            SoundFXManager.instance.PlaySoundEffect(playerHitAudioClip, transform, damage / 100);

            if (currentHealth <= 0)
            {
                GetComponent<PlayerUI>().OpenDeathScreen();
            }
        }

        else
        {
            SoundFXManager.instance.PlaySoundEffect(invulnHitAudioClip, transform, 0.4f);
        }

        playerRigidbody.AddForce(pushBackDirection * pushBackForce, ForceMode.Impulse);
        
        cinemachineImpulseSource.GenerateImpulseWithForce(screenShakeForce);
    }

    public void HealthRegenPowerUp(float health)
    {
        if(health + currentHealth > totalHealth)
        {
            currentHealth = totalHealth;
        }
        else
        {
            currentHealth += health;
        }
    }

    public void HandleGiantHealth(float health)
    {
        totalHealth = health;
        currentHealth = totalHealth;
    }
}