using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyAIManager enemyAIManager;
    private EnemyMovement enemyMovement;

    public enum AIState
    {
        Idle,
        Chasing,
        Searching
    }

    AIState currentState;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAIManager = GetComponent<EnemyAIManager>();
    }
    private void FixedUpdate()
    {
        enemyMovement.HandleEnemyMovement(enemyAIManager.movementDirection);
        enemyMovement.HandleEnemyTurning(enemyAIManager.movementDirection);
    }
}
