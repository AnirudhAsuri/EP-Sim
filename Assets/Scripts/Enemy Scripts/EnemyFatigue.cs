using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFatigue : MonoBehaviour
{
    private EnemyMovement enemyMovement;

    [SerializeField] private GameObject normalEyebrows;
    [SerializeField] private GameObject tiredEyebrows;
    public float totalFatigue;
    public float currentFatigue;

    [SerializeField] private float fatigueDrainRate;
    [SerializeField] private float fatigueRiseRate;

    private float maxEnemySpeed;
    private float enemySpeed;

    private bool isInTiredState;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();

        currentFatigue = totalFatigue;

        maxEnemySpeed = enemyMovement.maxSpeed;
        enemySpeed = enemyMovement.movementSpeed;
    }

    public void HandleFatigueChange()
    {
        if(enemyMovement.isWalking)
        {
            currentFatigue -= fatigueDrainRate * Time.deltaTime * enemyMovement.enemyRigidBody.velocity.magnitude;
        }
        else //When not walking
        {
            HandleFatigueRise();
        }

        currentFatigue = Mathf.Clamp(currentFatigue, 0f, totalFatigue);
    }

    private void HandleFatigueRise()
    {
        currentFatigue += fatigueRiseRate * Time.deltaTime;
    }

    public void HandleEnemyTiredState()
    {
        if(!isInTiredState)
        {
            isInTiredState = true;

            currentFatigue = 0f;
            normalEyebrows.SetActive(false);
            tiredEyebrows.SetActive(true);
            HandleTiredMovement();
        }
        
        HandleFatigueRise();  
    }

    private void HandleTiredMovement()
    {
        enemyMovement.maxSpeed = maxEnemySpeed / 2;

        enemyMovement.movementSpeed = enemySpeed / 2;
    }

    public void HandleTiredStateExit()
    {
        isInTiredState = false;
        normalEyebrows.SetActive(true);
        tiredEyebrows.SetActive(false);

        enemyMovement.maxSpeed = maxEnemySpeed;
        enemyMovement.movementSpeed = enemySpeed;
    }
}