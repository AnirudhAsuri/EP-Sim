using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerHealth : Health
{
    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;

    public float damageTaken;
    [SerializeField] private float cameraShakeDuration;

    private float screenShakeForce;

    [SerializeField] private AudioClip playerHitAudioClip;

    private void Start()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

        InitialiseTotalHealth();
    }

    public override void TakeDamage(float damage)
    {
        screenShakeForce = damage * 0.01f;  
        currentHealth -= damage;

        SoundFXManager.instance.PlaySoundEffect(playerHitAudioClip, transform, damage / 100);
        cinemachineImpulseSource.GenerateImpulseWithForce(screenShakeForce);
    }
}