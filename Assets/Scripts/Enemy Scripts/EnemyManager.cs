using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyAnimations enemyAnimations;
    private EnemyAIManager enemyAIManager;
    private EnemyMovement enemyMovement;
    private TargetDetectionSystem targetDetectionSystem;
    private EnemyDeath enemyDeath;
    private EnemyHealth enemyHealth;
    private EnemyAttacking enemyAttacking;

    private bool hasSeenPlayer = false;

    public enum AIState
    {
        Idle,
        Chasing
    }

    AIState currentState;

    private void Start()
    {
        currentState = AIState.Idle;

        enemyAnimations = GetComponentInChildren<EnemyAnimations>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAIManager = GetComponent<EnemyAIManager>();
        targetDetectionSystem = GetComponentInChildren<TargetDetectionSystem>();
        enemyDeath = GetComponent<EnemyDeath>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttacking = GetComponent<EnemyAttacking>();
    }

    private void FixedUpdate()
    {
        enemyMovement.HandleEnemyMovement(enemyAIManager.movementDirection);
        enemyAnimations.HandleEnemyWalkingAnimation();
        enemyAttacking.HandleEnemyAttacks();

        CheckForEnemyAwareness();

        switch(currentState)
        {
            case AIState.Idle:
                enemyAIManager.movementDirection = Vector3.zero;
                break;


            case AIState.Chasing:
                enemyAIManager.FinalAIUpdator();
                enemyMovement.HandleEnemyTurning(enemyAIManager.movementDirection);
                break;
        }

        if(enemyHealth.currentHealth <= 0)
        {
            enemyDeath.SwitchBodies();
        }
    }

    private void CheckForEnemyAwareness()
    {
        if (hasSeenPlayer)
            return;
        if(!hasSeenPlayer && targetDetectionSystem.targetInVision)
        {
            hasSeenPlayer = true;   
            StateSwitcher(AIState.Chasing);
        }
    }

    public void StateSwitcher(AIState newState)
    {
        if(currentState != newState)
        {
            currentState = newState;    
        }
    }
}
