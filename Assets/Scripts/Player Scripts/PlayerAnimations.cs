using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private GameObject leftAttackColliderObject;
    [SerializeField] private GameObject rightAttackColliderObject;

    private Animator playerAnimator;
    private PlayerMovement playerMovement;
    private PlayerGroundCheck playerGroundCheck;

    private string walkingParameter;
    private string sprintingParameter;
    private string groundedParameter;
    private string giantParameter;
    private string leftAttackTrigger;
    private string rightAttackTrigger;

    [SerializeField] private AudioClip punchAirAudioClip;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerGroundCheck = GetComponentInParent<PlayerGroundCheck>();

        walkingParameter = "IsWalking";
        sprintingParameter = "IsSprinting";
        groundedParameter = "IsGrounded";
        giantParameter = "IsGiant";
        leftAttackTrigger = "Left Attack Trigger";
        rightAttackTrigger = "Right Attack Trigger";
    }
    public void HandleWalkingAnimations()
    {
        playerAnimator.SetBool(walkingParameter, playerMovement.isWalking);

        playerAnimator.SetBool(sprintingParameter, playerMovement.isSprinting);
        
        playerAnimator.SetBool(groundedParameter, playerGroundCheck.isGrounded);
    }

    public void HandleGiantAnimations(bool isGiant)
    {
        playerAnimator.SetBool(giantParameter, isGiant);
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