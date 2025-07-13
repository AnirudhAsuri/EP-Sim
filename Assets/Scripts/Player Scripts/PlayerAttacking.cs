using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacking : MonoBehaviour
{
    private String EnemyTag;

    private PlayerAnimations playerAnimations;
    private PlayerMovement playerMovement;

    public InputActionReference leftAttack;
    public InputActionReference rightAttack;

    [SerializeField] private int attackDamageMultiplier;
    private int attackDamage;
    public float pushBackMeasure;
    public Vector3 pushBackDirection;
    [SerializeField] private int basePushBack;
    [SerializeField] private int baseAttackDamage;
    [SerializeField] private float pushBackMultiplier;

    private void Start()
    {
        playerAnimations = GetComponentInChildren<PlayerAnimations>();
        playerMovement = GetComponent<PlayerMovement>();

        attackDamage = baseAttackDamage;
        pushBackMeasure = basePushBack;

        EnemyTag = "Enemy";
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
    }
}