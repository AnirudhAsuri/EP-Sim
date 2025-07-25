using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDummyEnemyManager : MonoBehaviour
{
    private EnemyDeath enemyDeath;
    private EnemyHealth enemyHealth;
    private Tutorial tutorial;

    private void Start()
    {
        tutorial = FindAnyObjectByType<Tutorial>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyDeath = GetComponent<EnemyDeath>();
    }

    private void Update()
    {
        if(enemyHealth.currentHealth <= 100 && tutorial.currentTextPointer == 8)
        {
            HandleNextStepIncrement();
        }

        if(enemyHealth.currentHealth <= 0f)
        {
            HandleNextStepIncrement(); //Current Text Index = 12
            enemyDeath.HandlePlayerHealthRegen();
            enemyDeath.SwitchBodies();
        }
    }

    private void HandleNextStepIncrement()
    {
        tutorial.IncrementCurrentTextPointer();
    }
}
