using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    [SerializeField] private Animator enemyAnimator;

    [SerializeField] private GameObject leftPunchCollider;
    [SerializeField] private GameObject rightPunchCollider;

    private string walkingParameter;
    private string rightPunchTrigger;
    private string leftPunchTrigger;

    private void Start()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();

        walkingParameter = "IsWalking";
        rightPunchTrigger = "Right Punch";
        leftPunchTrigger = "Left Punch";
    }

    public void HandleEnemyWalkingAnimation()
    {
        enemyAnimator.SetBool(walkingParameter, enemyMovement.isWalking);
    }

    public void EnemyRightPunch()
    {
        enemyAnimator.SetTrigger(rightPunchTrigger);
    }

    public void EnemyLeftPunch()
    {
        enemyAnimator.SetTrigger(leftPunchTrigger);
    }

    public void ActivateLeftAttackCollider()
    {
        leftPunchCollider.SetActive(true);
    }

    public void DeactivateLeftAttackCollider()
    {
        leftPunchCollider.SetActive(false);
    }

    public void ActivateRightAttackCollider()
    {
        rightPunchCollider.SetActive(true);
    }

    public void DeactivateRightAttackCollider()
    {
        rightPunchCollider.SetActive(false);
    }
}
