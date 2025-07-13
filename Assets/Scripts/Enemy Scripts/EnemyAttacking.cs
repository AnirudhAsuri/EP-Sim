using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    private EnemyAnimations enemyAnimations;
    private EnemyAIManager enemyAIManager;
    private TargetDetectionSystem targetDetectionSystem;

    private string playerTag;
    [SerializeField] private float minimumAttackDistance;

    [SerializeField] private int attackDamageMultiplier;
    [SerializeField] private int baseAttackDamage;
    [SerializeField] private int basePushBack;
    [SerializeField] private float pushBackMultiplier;

    private int attackDamage;
    public float pushBackMeasure;
    public Vector3 pushBackDirection;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAIManager = GetComponent<EnemyAIManager>();
        targetDetectionSystem = GetComponentInChildren<TargetDetectionSystem>();
        enemyAnimations = GetComponentInChildren<EnemyAnimations>();

        playerTag = "Player";
        attackDamage = baseAttackDamage;
        pushBackMeasure = basePushBack;
    }

    public void HandleEnemyAttacks()
    {
        float distanceToTarget = enemyAIManager.directionToTarget.magnitude;
        Vector3 dirToTarget = enemyAIManager.directionToTarget.normalized;

        if (!targetDetectionSystem.targetInVision)
            return;
        if(distanceToTarget <= minimumAttackDistance)
        {
            float sideDot = Vector3.Dot(transform.right, dirToTarget);
            float frontDot = Vector3.Dot(transform.forward, dirToTarget);

            if(frontDot > 0f)
            {
                if (sideDot > 0f)
                {
                    enemyAnimations.EnemyLeftPunch();
                }

                else if (sideDot < 0f)
                {
                    enemyAnimations.EnemyRightPunch();
                }

                else if (sideDot == 0f)
                {
                    enemyAnimations.EnemyRightPunch();
                }
            }
        }    
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag))
        {
            HandleDamageCalculations();

            Vector3 directionToTarget = transform.position - other.transform.position;
            pushBackDirection = -directionToTarget.normalized;

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if(playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                other.attachedRigidbody.AddForce(pushBackMeasure * pushBackDirection, ForceMode.Impulse);
            }
            
        }
        pushBackMeasure = basePushBack;
        attackDamage = baseAttackDamage;
    }

    private void HandleDamageCalculations()
    {
        int speed = (int)enemyMovement.enemyRigidBody.velocity.magnitude;
        pushBackMeasure = basePushBack + (pushBackMultiplier * attackDamage);

        attackDamage += attackDamageMultiplier * speed;
    }
}