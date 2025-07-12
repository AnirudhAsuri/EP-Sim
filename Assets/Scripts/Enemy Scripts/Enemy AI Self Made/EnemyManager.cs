using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyAIManager enemyAIManager;
    private EnemyMovement enemyMovement;
    private TargetDetectionSystem targetDetectionSystem;

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

        enemyMovement = GetComponent<EnemyMovement>();
        enemyAIManager = GetComponent<EnemyAIManager>();
        targetDetectionSystem = GetComponentInChildren<TargetDetectionSystem>();
    }
    private void FixedUpdate()
    {
        enemyMovement.HandleEnemyMovement(enemyAIManager.movementDirection);
        

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
