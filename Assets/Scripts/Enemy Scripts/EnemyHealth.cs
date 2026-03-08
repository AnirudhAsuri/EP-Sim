using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : Health
{
    public float pushBackMeasure;
    public Vector3 pushBackDirection;

    [SerializeField] private AudioClip hitAudioClip;

    public static List<EnemyHealth> AllEnemies = new List<EnemyHealth>();

    private void Awake()
    {
        InitialiseTotalHealth();
    }

    public override void TakeDamage(float damage)
    {
        currentHealth -= damage;

        SoundFXManager.instance.PlaySoundEffect(hitAudioClip, transform, damage/100);
    }

    private void OnEnable()
    {
        if(!AllEnemies.Contains(this))
        {
            AllEnemies.Add(this);
        }
    }

    private void OnDisable()
    {
        AllEnemies.Remove(this);
    }

    private void OnDestroy()
    {
        AllEnemies.Remove(this);
    }
}
