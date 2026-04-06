using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private PlayerGroundCheck playerGroundCheck;
    private PowerUpEffects powerUpEffects;
    private Transform cameraTranform;

    public Rigidbody playerRigidBody;

    [SerializeField] private InputActionReference movementAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference sprintAction;

    public Vector3 movementInput;
    public Vector3 movementDir;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintQuoficient;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rotationSpeed;

    private ParticleSystem dustParticleSystem;
    [SerializeField] private float minimumDustSpeed;

    public bool isWalking = false;
    public bool isSprinting = false;

    private float sandyMovementSpeed = 8f;
    private float grassyMovementSpeed = 10f;
    private float icyMovementSpeed = 13f;
    private float defaultMovementSpeed = 10f;

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerGroundCheck = GetComponent<PlayerGroundCheck>();
        powerUpEffects = GetComponentInChildren<PowerUpEffects>();
        cameraTranform = Camera.main.transform;

        dustParticleSystem = GetComponentInChildren<ParticleSystem>();

        if (dustParticleSystem != null)
        {
            var emission = dustParticleSystem.emission;
            emission.enabled = false;
        }
    }

    private void Update()
    {
        Vector2 input = movementAction.action.ReadValue<Vector2>();

        movementInput = new Vector3(input.x, movementInput.y, input.y);

        if(!isWalking)
        {
            isSprinting = false;
        }

        if(SprintButtonPopAndActivate.Instance != null)
        {
            SprintButtonPopAndActivate.Instance.ChangeColor(isSprinting);
        }
    }

    public void HandleMovementDirection()
    {
        movementDir = cameraTranform.forward * movementInput.z + cameraTranform.right * movementInput.x;
        movementDir.y = 0f;

        if(movementDir.magnitude > 0.1f)
        {
            movementDir.Normalize();
        }
    }

    public void HandlePlayerMovement()
    {
        float activeMaxSpeed = maxSpeed;

        if(powerUpEffects.isSpedUp)
        {
            activeMaxSpeed *= powerUpEffects.powerUpSpeedLimitMultiplier;
        }

        Vector3 playerHorizontalVelocity = new Vector3(playerRigidBody.velocity.x, 0f, playerRigidBody.velocity.z);

        if (isSprinting)
        {
            Vector3 sprintForce = movementDir * movementSpeed * sprintQuoficient;

            playerRigidBody.AddForce(sprintForce, ForceMode.Acceleration);
        }

        else
        {
            Vector3 walkForce = movementDir * movementSpeed;

            playerRigidBody.AddForce(walkForce, ForceMode.Acceleration);

            if(playerHorizontalVelocity.magnitude > activeMaxSpeed)
            {
                float excessSpeed = playerHorizontalVelocity.magnitude - activeMaxSpeed;

                Vector3 breakForce = -playerHorizontalVelocity.normalized * excessSpeed;
                playerRigidBody.AddForce(breakForce, ForceMode.Acceleration);
            }
        }

        isWalking = movementInput.magnitude > 0.1f;
    }

    public void HandlePlayerMovementSpeed()
    {
        switch(playerGroundCheck.currentFloorType)
        {
            case PlayerGroundCheck.FloorType.GrassyFloor:
                movementSpeed = grassyMovementSpeed;
                break;

            case PlayerGroundCheck.FloorType.SandyFloor:
                if (powerUpEffects.ignoreSand)
                    movementSpeed = defaultMovementSpeed;
                else
                    movementSpeed = sandyMovementSpeed;
                break;

            case PlayerGroundCheck.FloorType.IcyFloor:
                if (powerUpEffects.ignoreIce)
                    movementSpeed = defaultMovementSpeed;
                else
                    movementSpeed = icyMovementSpeed;
                break;

            default:
                movementSpeed = defaultMovementSpeed;
                break;
        }

        if(powerUpEffects.isSpedUp)
        {
            movementSpeed *= powerUpEffects.powerUpSpeedMultiplier;
        }
    }

    public void HandlePlayerTurning(Vector3 movementDirection)
    {
        if (playerRigidBody.velocity.magnitude > 0.1f)
        {
            float targetAngle;

            targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            playerRigidBody.MoveRotation(Quaternion.Slerp(playerRigidBody.rotation, targetRotation, rotationSpeed * Time.deltaTime));
        }
    }

    public void HandleSprintDust()
    {
        if (dustParticleSystem == null) return;

        var emission = dustParticleSystem.emission;

        if ((playerRigidBody.velocity.magnitude < minimumDustSpeed) || !playerGroundCheck.isGrounded)
        {
            emission.enabled = false;
        }
        else
        {
            emission.enabled = true;
        }
    }

    private void OnEnable()
    {
        movementAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();

        jumpAction.action.started += Jump;

        sprintAction.action.started += SprintStatus;
    }

    private void OnDisable()
    {
        jumpAction.action.started -= Jump;

        sprintAction.action.started -= SprintStatus;
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if(playerGroundCheck.isGrounded)
        {
            playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            JumpButtonPop.Instance.ButtonPop();
        }
    }

    private void SprintStatus(InputAction.CallbackContext obj)
    {
        isSprinting = !isSprinting;

        SprintButtonPopAndActivate.Instance.ButtonPop();
    }
}