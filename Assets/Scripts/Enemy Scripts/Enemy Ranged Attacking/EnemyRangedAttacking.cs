using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttacking : MonoBehaviour
{
    private Rigidbody enemyRigidBody;
    private Animator rangedEnemyAnimator;
    private EnemyFatigue enemyFatigue;
    private EnemyAIManager enemyAIManager;
    private TargetDetectionSystem targetDetectionSystem;

    [SerializeField] private Transform bulletSpawningPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float maximumAttackDistance;
    [SerializeField] private float fatigueLossValue;

    [SerializeField] private float recoilPushBack;
    private Vector3 recoilDirection;
    private float targetOffset = 1f;

    public float minimumMaintainedDistance;
    [SerializeField] private float gunLength;
    [SerializeField] private float timeBetweenShots;
    private float timeSinceShot = 1.5f;

    [SerializeField] private AudioClip shootingAudioClip;

    private string shootTrigger = "Shoot";

    private void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody>();
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

        if (distanceToTarget <= maximumAttackDistance && distanceToTarget >= gunLength)
        {
            timeSinceShot += Time.deltaTime;
            if (timeSinceShot >= timeBetweenShots)
            {
                FireGun();

                if(recoilPushBack >= 0.1f)
                {
                    recoilDirection = -transform.forward;
                    enemyRigidBody.AddForce(recoilDirection * recoilPushBack, ForceMode.Impulse);
                }

                timeSinceShot = 0f;
            }
        }
    }

    public void FireGun()
    {   
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawningPoint.position, bulletSpawningPoint.rotation);

        rangedEnemyAnimator.SetTrigger(shootTrigger);
        enemyFatigue.HandleFatigueLoss(fatigueLossValue);
        SoundFXManager.instance.PlaySoundEffect(shootingAudioClip, transform, 0.6f);

        Vector3 targetPosition = new Vector3(enemyAIManager.targetPosition.x, enemyAIManager.targetPosition.y + targetOffset, enemyAIManager.targetPosition.z);
        Vector3 fireDirection = (targetPosition - bulletSpawningPoint.position).normalized;

        bullet.transform.forward = fireDirection;

        bullet.GetComponent<Rigidbody>().velocity = fireDirection * bulletSpeed;
    }
}