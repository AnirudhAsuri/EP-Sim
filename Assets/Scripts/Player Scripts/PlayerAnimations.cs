using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private GameObject leftAttackColliderObject;
    [SerializeField] private GameObject rightAttackColliderObject;

    private Animator playerAnimator;
    private PlayerMovement playerMovement;
    private PlayerGroundCheck playerGroundCheck;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerGroundCheck = GetComponentInParent<PlayerGroundCheck>();
    }
    public void HandleWalkingAnimations()
    {
        playerAnimator.SetBool("IsWalking", playerMovement.isWalking);

        playerAnimator.SetBool("IsGrounded", playerGroundCheck.isGrounded);
    }

    public void ActivateLeftAttack()
    {
        playerAnimator.SetTrigger("Left Attack Trigger");
    }

    public void ActivateRightAttack()
    {
        playerAnimator.SetTrigger("Right Attack Trigger");
    }

    public void ActivateLeftAttackCollider()
    {
        leftAttackColliderObject.SetActive(true);
    }

    public void DeactivateLeftAttackCollider()
    {
        leftAttackColliderObject.SetActive(false);
    }

    public void ActivateRightAttackCollider()
    {
        rightAttackColliderObject.SetActive(true);
    }

    public void DeactivateRightAttackCollider()
    {
        rightAttackColliderObject.SetActive(false);
    }

}