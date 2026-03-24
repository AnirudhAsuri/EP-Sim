using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttacking : MonoBehaviour
{
    private Animator rangedEnemyAnimator;
    private EnemyFatigue enemyFatigue;
    private EnemyAIManager enemyAIManager;
    private TargetDetectionSystem targetDetectionSystem;

    [SerializeField] private Transform bulletSpawningPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float maximumAttackDistance;
    [SerializeField] private float fatigueLossValue;

    public float minimumMaintainedDistance;
    [SerializeField] private float timeBetweenShots;
    private float timeSinceShot = 1.5f;

    [SerializeField] private AudioClip shootingAudioClip;

    private string shootTrigger = "Shoot";

    private void Start()
    {
        rangedEnemyAnimator = GetComponentInChildren<Animator>();
        enemyAIManager = GetComponent<EnemyAIManager>();
        enemyFatigue = GetComponent<EnemyFatigue>();
        targetDetectionSystem = GetComponentInChildren<TargetDetectionSystem>();
    }

    public void HandleGunShooting()
    {
        float distanceToTarget = enemyAIManager.distanceToTarget;

        if (!targetDetectionSystem.targetInVision)
            return;

        if (distanceToTarget <= maximumAttackDistance)
        {
            timeSinceShot += Time.deltaTime;
            if (timeSinceShot >= timeBetweenShots)
            {
                FireGun();
                timeSinceShot = 0f;
            }
        }
    }

    public void FireGun()
    {   
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawningPoint.position, bulletSpawningPoint.rotation);
        rangedEnemyAnimator.SetTrigger(shootTrigger);
        enemyFatigue.HandleFatigueLoss(fatigueLossValue);
        SoundFXManager.instance.PlaySoundEffect(shootingAudioClip, transform, 1);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawningPoint.forward * bulletSpeed;
    }
}