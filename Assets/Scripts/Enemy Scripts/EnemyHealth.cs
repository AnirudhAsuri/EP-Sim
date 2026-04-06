using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : Health
{
    private Rigidbody enemyRigidbody;

    public float pushBackMeasure;
    public Vector3 pushBackDirection;
    public float pushBackForce;

    [SerializeField] private AudioClip hitAudioClip;

    public static List<EnemyHealth> AllEnemies = new List<EnemyHealth>();

    private void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();

        InitialiseTotalHealth();
    }

    public override void TakeDamage(float damage, Vector3 direction, float force)
    {
        currentHealth -= damage;

        pushBackDirection = direction;
        pushBackForce = force;

        enemyRigidbody.AddForce(pushBackDirection * pushBackForce, ForceMode.Impulse);

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
