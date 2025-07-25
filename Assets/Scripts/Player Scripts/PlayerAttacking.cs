using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerAttacking : MonoBehaviour
{
    private string EnemyTag;

    private CinemachineImpulseSource cinemachineImpulseSource;
    private PlayerAnimations playerAnimations;
    private PlayerMovement playerMovement;

    public InputActionReference leftAttack;
    public InputActionReference rightAttack;

    [SerializeField] private float attackDamageMultiplier;
    private float attackDamage;
    public float pushBackMeasure;
    public Vector3 pushBackDirection;
    [SerializeField] private float basePushBack;
    [SerializeField] private float baseAttackDamage;
    [SerializeField] private float pushBackMultiplier;

    private float screenShakeForce;

    private GameObject hitParticlesPrefab;
    private ParticleSystem hitParticles;
    private ParticleSystem hitParticlesInstance;

    private void Start()
    {
        playerAnimations = GetComponentInChildren<PlayerAnimations>();
        playerMovement = GetComponent<PlayerMovement>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

        attackDamage = baseAttackDamage;
        pushBackMeasure = basePushBack;

        EnemyTag = "Enemy";

        hitParticlesPrefab = Resources.Load<GameObject>("Particles/HitParticles");

        if (hitParticlesPrefab != null)
        {
            hitParticles = hitParticlesPrefab.GetComponent<ParticleSystem>();
        }
    }

    private void OnEnable()
    {
        leftAttack.action.started += LeftAttack;

        rightAttack.action.started += RightAttack;
    }

    private void OnDisable()
    {
        leftAttack.action.started -= LeftAttack;

        rightAttack.action.started -= RightAttack;
    }

    private void RightAttack(InputAction.CallbackContext obj)
    {
        playerAnimations.RightAttack();
    }

    private void LeftAttack(InputAction.CallbackContext obj)
    {
        playerAnimations.LeftAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(EnemyTag))
        {
            HandleDamageCalculations();

            Vector3 directionToTarget = transform.position - other.transform.position;
            pushBackDirection = -directionToTarget.normalized;
            pushBackDirection.y = 0;

            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                other.attachedRigidbody.AddForce(pushBackMeasure * pushBackDirection, ForceMode.Impulse);
                Debug.Log(pushBackMeasure);

                if(hitParticles != null)
                {
                    Vector3 hitPosition = other.ClosestPoint(transform.position);

                    hitParticlesInstance = Instantiate(hitParticles, hitPosition, Quaternion.identity);
                }
            }

            TargetDetectionSystem detectionSystem = other.GetComponentInChildren<TargetDetectionSystem>();
            if (detectionSystem != null)
            {
                detectionSystem.AlertFromAttack(transform.position);
            }
        }

        attackDamage = baseAttackDamage;
        pushBackMeasure = basePushBack;
    }

    private void HandleDamageCalculations()
    {
        int playerSpeed;
        if (playerMovement.isWalking)
            playerSpeed = (int)playerMovement.playerRigidBody.velocity.magnitude;
        else
            playerSpeed = 1;

        attackDamage += attackDamageMultiplier * playerSpeed;
        pushBackMeasure += (pushBackMultiplier * attackDamage);

        screenShakeForce = attackDamage * 0.01f;

        cinemachineImpulseSource.GenerateImpulseWithForce(screenShakeForce);
    }
}