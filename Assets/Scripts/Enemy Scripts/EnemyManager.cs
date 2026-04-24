using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int meleeEnemyLayer;
    public int rangedEnemyLayer;

    private EnemyAnimations enemyAnimations;
    private EnemyAIManager enemyAIManager;
    private EnemyMovement enemyMovement;
    private TargetDetectionSystem targetDetectionSystem;
    private EnemyDeath enemyDeath;
    private EnemyHealth enemyHealth;
    private EnemyAttacking enemyAttacking;
    private EnemyRangedAttacking enemyRangedAttacking;
    private EnemyFatigue enemyFatigue;

    private bool hasSeenPlayer = false;

    public enum AIState
    {
        Idle,
        Chasing,
        Tired
    }

    AIState currentState;

    private void Awake()
    {
        currentState = AIState.Idle;

        enemyAnimations = GetComponentInChildren<EnemyAnimations>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAIManager = GetComponent<EnemyAIManager>();
        targetDetectionSystem = GetComponentInChildren<TargetDetectionSystem>();
        enemyDeath = GetComponent<EnemyDeath>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttacking = GetComponent<EnemyAttacking>();
        enemyRangedAttacking = GetComponent<EnemyRangedAttacking>();
        enemyFatigue = GetComponent<EnemyFatigue>();

        meleeEnemyLayer = LayerMask.NameToLayer("Melee Enemy");
        rangedEnemyLayer = LayerMask.NameToLayer("Ranged Enemy");

        if(enemyDeath == null)
        {
            Debug.Log("Hull");
        }
    }

    private void FixedUpdate()
    {
        enemyMovement.HandleEnemyMovement(enemyAIManager.movementDirection);
        enemyAnimations.HandleEnemyWalkingAnimation();
        
        if(currentState != AIState.Tired)
        {
            if(enemyAttacking != null)
                enemyAttacking.HandleEnemyAttacks();
            if (enemyRangedAttacking != null)
                enemyRangedAttacking.HandleGunShooting();
        }
            
        CheckForEnemyAwareness();

        CheckForTiredState();

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
                enemyAIManager.FinalAIUpdator();
                enemyFatigue.HandleEnemyTiredState();
                enemyMovement.HandleEnemyMovement(enemyAIManager.movementDirection);
                enemyMovement.HandleEnemyTurning(enemyAIManager.movementDirection);

                if(CheckIfEnemyHasRecoveredFromFatigue())
                {
                    enemyFatigue.HandleTiredStateExit();
                    StateSwitcher(AIState.Chasing);
                }
                break;
        }

        if(enemyHealth.currentHealth <= 0)
        {
            Vector3 finalPushbackDirection = enemyHealth.pushBackDirection;
            float finalPushbackForce = enemyHealth.pushBackForce;

            enemyDeath.SwitchBodies(finalPushbackDirection, finalPushbackForce);
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

    private bool CheckIfEnemyHasRecoveredFromFatigue()
    {
        return enemyFatigue.currentFatigue >= enemyFatigue.totalFatigue;
    }

    private void CheckForTiredState()
    {
        if(enemyFatigue.currentFatigue <= 0f)
        {
            StateSwitcher(AIState.Tired);
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
