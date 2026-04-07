using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.PostProcessing;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private GameObject enemyDeadBody;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;
    private bool isDead = false;

    private string levelMusicTag = "Level Music Source";
    [SerializeField] private AudioClip crunchSound;

    private Vector3 deathPushBackDirection;

    [SerializeField] private float giantPowerUpProb;
    [SerializeField] private float healthRegenPowerUpProb;
    [SerializeField] private float icePowerUpProb;
    [SerializeField] private float sandPowerUpProb;
    [SerializeField] private float invulnerabilityPowerUpProb;
    [SerializeField] private float speedPowerUpProb;
    [SerializeField] private float nothingProb;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        playerHealth = FindObjectOfType<PlayerHealth>();

        GameObject levelMusicSourceObject = GameObject.FindGameObjectWithTag(levelMusicTag);
    }

    private void Start()
    {
        isDead = false;
    }

    public void HandlePlayerHealthRegen()
    {
        playerHealth.currentHealth += enemyHealth.totalHealth * 0.1f;
    }

    public void SwitchBodies(Vector3 pushDirection, float force)
    {
        if (isDead)
            return;

        isDead = true;

        GameObject deadBody = Instantiate(enemyDeadBody, transform.position, transform.rotation);
        Rigidbody deadBodyRigidBody = deadBody.GetComponent<Rigidbody>();

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        PowerUpManager.Instance.RevealPowerUp(transform, nothingProb, giantPowerUpProb, 
            healthRegenPowerUpProb, icePowerUpProb, sandPowerUpProb, invulnerabilityPowerUpProb, speedPowerUpProb);

        deadBodyRigidBody.AddForce(pushDirection * force, ForceMode.Impulse);

        PlayDeathSound();
        Destroy(gameObject);
    }

    private void PlayDeathSound()
    {
        SoundFXManager.instance.PlaySoundEffect(crunchSound, transform, 0.3f);
    }
}
