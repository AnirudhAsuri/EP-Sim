using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float life = 3;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float bulletPushbackMeasure;
    private Vector3 pushBackDirection;
    private bool damageIsDealt = false;

    private GameObject hitParticlesPrefab;
    private ParticleSystem hitParticles;
    private ParticleSystem hitParticlesInstance;

    [SerializeField] private AudioClip bulletHitSoundEffect;
 
    private void Start()
    {
        hitParticlesPrefab = Resources.Load<GameObject>("Particles/HitParticles");

        if (hitParticlesPrefab != null)
        {
            hitParticles = hitParticlesPrefab.GetComponent<ParticleSystem>();
        }
    }

    private void Awake()
    {
        Destroy(gameObject, life);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damageIsDealt)
            return;
        if (other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            pushBackDirection = transform.forward;

            GameObject player = other.gameObject;
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(bulletDamage, pushBackDirection, bulletPushbackMeasure);
                damageIsDealt = true;

                PowerUpEffects powerUpEffects = other.GetComponentInChildren<PowerUpEffects>();

                if (hitParticles != null && !powerUpEffects.isInvulnerable)
                {
                    Vector3 hitPosition = other.ClosestPoint(transform.position);

                    hitParticlesInstance = Instantiate(hitParticles, hitPosition, Quaternion.identity);

                    SoundFXManager.instance.PlaySoundEffect(bulletHitSoundEffect, player.transform, 0.8f);
                }
            }
        }

        Destroy(gameObject);
    }
}
