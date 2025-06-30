using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    [SerializeField] private Animator enemyAnimator;

    private void Start()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();
    }

    public void HandleEnemyWalkingAnimation()
    {
        enemyAnimator.SetBool("IsWalking", enemyMovement.isWalking);
    }
}
