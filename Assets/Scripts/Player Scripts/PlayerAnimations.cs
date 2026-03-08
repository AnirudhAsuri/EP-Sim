using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private GameObject leftAttackColliderObject;
    [SerializeField] private GameObject rightAttackColliderObject;

    private Animator playerAnimator;
    private PlayerMovement playerMovement;
    private PlayerGroundCheck playerGroundCheck;

    private string walkingParameter;
    private string groundedParameter;
    private string leftAttackTrigger;
    private string rightAttackTrigger;

    private string idleLeftAttack;
    private string idleRightAttack;
    private string runningLeftAttack;
    private string runningRightAttack;

    [SerializeField] private AudioClip punchAirAudioClip;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerGroundCheck = GetComponentInParent<PlayerGroundCheck>();

        walkingParameter = "IsWalking";
        groundedParameter = "IsGrounded";
        leftAttackTrigger = "Left Attack Trigger";
        rightAttackTrigger = "Right Attack Trigger";
    }
    public void HandleWalkingAnimations()
    {
        playerAnimator.SetBool(walkingParameter, playerMovement.isWalking);

        playerAnimator.SetBool(groundedParameter, playerGroundCheck.isGrounded);
    }

    public void LeftAttack()
    {
        playerAnimator.SetTrigger(leftAttackTrigger);
    }

    public void RightAttack()
    {
        playerAnimator.SetTrigger(rightAttackTrigger);
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

    public void PlayPunchSoundEffect()
    {
        SoundFXManager.instance.PlaySoundEffect(punchAirAudioClip, transform, 0.3f);
    }
}