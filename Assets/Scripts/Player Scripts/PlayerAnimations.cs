using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
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
}