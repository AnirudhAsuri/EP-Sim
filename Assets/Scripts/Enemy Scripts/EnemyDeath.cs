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
    private EnemyCount enemyCount;
    private bool isDead = false;

    private string levelMusicTag = "Level Music Source";
    private AudioSource levelMusicSource;
    private float freezeFrameDuration = 0.5f;
    [SerializeField] private AudioClip crunchSound;

    private Vector3 deathPushBackDirection;

    private PostProcessVolume postProcessVolume;
    private Vignette vignette;
    private Grain grain;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        enemyCount = FindObjectOfType<EnemyCount>();

        GameObject levelMusicSourceObject = GameObject.FindGameObjectWithTag(levelMusicTag);
        levelMusicSource = levelMusicSourceObject.GetComponent<AudioSource>();

        postProcessVolume = FindAnyObjectByType<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out grain);

        if (vignette == null)
        {
            Debug.Log("Hull");
        }
    }

    private void Start()
    {
        isDead = false;
    }

    public void HandlePlayerHealthRegen()
    {
        playerHealth.currentHealth += enemyHealth.totalHealth * 0.1f;
    }

    public void SwitchBodies()
    {
        if (isDead)
            return;

        isDead = true;

        GameObject deadBody = Instantiate(enemyDeadBody, transform.position, transform.rotation);

        deathPushBackDirection = new Vector3(enemyHealth.pushBackDirection.x, 0.2f, enemyHealth.pushBackDirection.z);

        Rigidbody deadBodyRigidBody = deadBody.GetComponent<Rigidbody>();

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        PlayDeathSound();

        if (enemyCount.enemyCount > 1)
        {
            StartCoroutine(FreezeFrameAndDestroyObject(freezeFrameDuration));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PlayDeathSound()
    {
        SoundFXManager.instance.PlaySoundEffect(crunchSound, transform, 0.3f);
    }

    private IEnumerator FreezeFrameAndDestroyObject(float duration)
    {
        if(levelMusicSource != null)
        {
            levelMusicSource.Pause();
        }

        if(vignette != null)
        {
            vignette.enabled.value = true;
        }
        
        if(grain != null)
        {
            grain.enabled.value = true;
        }

        Time.timeScale = 0.05f;
        
        yield return new WaitForSecondsRealtime(duration);

        if(vignette != null)
        {
            vignette.enabled.value = false;
        }

        if (grain != null)
        {
            grain.enabled.value = false;
        }

        if (levelMusicSource != null)
        {
            levelMusicSource.Play();
        }

        Time.timeScale = 1f;
        Destroy(gameObject);
    }
}
