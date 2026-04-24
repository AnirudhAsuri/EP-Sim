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

    private bool sameEffect = false;

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
                if (effects.isGiant)
                {
                    sameEffect = true;
                    break;
                }
                effects.MakeGiant();
                break;

            case Type.Speed:
                if (effects.isSpedUp)
                {
                    sameEffect = true;
                    break;
                }
                effects.ActivateSpeedPowerUp();
                break;

            case Type.Health:
                effects.RegenHealth();
                break;

            case Type.Invulnerability:
                if (effects.isInvulnerable)
                {
                    sameEffect = true;
                    break;
                }
                effects.ActivateInvulnerability(effects.invulnerabilityDuration);
                break;

            case Type.Sand:
                if (effects.ignoreSand)
                {
                    sameEffect = true;
                    break;
                }
                effects.ActivateIgnoreSand();
                break;

            case Type.Ice:
                if (effects.ignoreIce)
                {
                    sameEffect = true;
                    break;
                }
                effects.ActivateIgnoreIce();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (time < 0.5f)
            return;


        if(other.CompareTag(playerTag))
        {
            ApplyEffect(other.GetComponentInChildren<PowerUpEffects>());

            if (sameEffect)
            {
                sameEffect = false;
                return;
            }

            Destroy(gameObject);

            SoundFXManager.instance.PlaySoundEffect(powerUpSoundEffect, transform, soundFXVolume);
            SoundFXManager.instance.PlaySoundEffect(indivisualSoundEffect, other.transform, soundFXVolume);
            
            var renderer = particleInstance.GetComponent<ParticleSystemRenderer>();
            renderer.material = particleSystemPowerUpMat;
            renderer.trailMaterial = particleSystemPowerUpMat;
            particleInstance = Instantiate(powerUpParticles, transform.position, Quaternion.identity);
        }
    }
}
