using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    public enum Type { Giant, Speed, Health, Invulnerability, Sand, Ice }
    public Type powerUpType;

    private string playerTag = "Player";
    private float time = 0f;

    [SerializeField] private ParticleSystem powerUpParticles;
    private ParticleSystem particleInstance;
    [SerializeField] private Material particleSystemPowerUpMat;

    [SerializeField] private AudioClip powerUpSoundEffect;
    [SerializeField] private AudioClip indivisualSoundEffect;
    private float soundFXVolume = 0.5f;

    private void Start()
    {
        particleInstance = powerUpParticles;
        
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    public void ApplyEffect(PowerUpEffects effects)
    {
        switch (powerUpType)
        {
            case Type.Giant:
                effects.MakeGiant();
                break;

            case Type.Speed:
                effects.ActivateSpeedPowerUp();
                break;

            case Type.Health:
                effects.RegenHealth();
                break;

            case Type.Invulnerability:
                effects.ActivateInvulnerability(effects.invulnerabilityDuration);
                break;

            case Type.Sand:
                effects.ActivateIgnoreSand();
                break;

            case Type.Ice:
                effects.ActivateIgnoreIce();
                break;
        }

        Destroy(gameObject); // Cleanup
    }

    private void OnTriggerEnter(Collider other)
    {
        if (time < 0.5f)
            return;

        if(other.CompareTag(playerTag))
        {
            ApplyEffect(other.GetComponentInChildren<PowerUpEffects>());

            SoundFXManager.instance.PlaySoundEffect(powerUpSoundEffect, transform, soundFXVolume);
            SoundFXManager.instance.PlaySoundEffect(indivisualSoundEffect, other.transform, soundFXVolume);
            
            var renderer = particleInstance.GetComponent<ParticleSystemRenderer>();
            renderer.material = particleSystemPowerUpMat;
            renderer.trailMaterial = particleSystemPowerUpMat;
            particleInstance = Instantiate(powerUpParticles, transform.position, Quaternion.identity);
        }
    }
}
