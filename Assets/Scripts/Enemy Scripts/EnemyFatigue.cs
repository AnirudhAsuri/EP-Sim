using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFatigue : MonoBehaviour
{
    private EnemyMovement enemyMovement;

    [SerializeField] private GameObject normalEyebrows;
    [SerializeField] private GameObject tiredEyebrows;
    [SerializeField] private float totalFatigue;
    [SerializeField] private float currentFatigue;

    [SerializeField] private float fatigueDrainRate;
    [SerializeField] private float fatigueRiseRate;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();

        currentFatigue = totalFatigue;
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
        currentFatigue = 0f;
        HandleFatigueRise();

        normalEyebrows.SetActive(false);
        tiredEyebrows.SetActive(true);
    }

    private void HandleTiredMovement()
    {

    }
}