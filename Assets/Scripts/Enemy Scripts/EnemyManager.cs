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
    private EnemyFatigue enemyFatigue;

    private bool hasSeenPlayer = false;

    public enum AIState
    {
        Idle,
        Chasing,
        Tired
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
        enemyFatigue = GetComponent<EnemyFatigue>();
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
                enemyFatigue.HandleFatigueChange();
                break;


            case AIState.Chasing:
                enemyAIManager.FinalAIUpdator();
                enemyFatigue.HandleFatigueChange();
                enemyMovement.HandleEnemyTurning(enemyAIManager.movementDirection);
                break;

            case AIState.Tired:
                enemyFatigue.HandleEnemyTiredState();
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
