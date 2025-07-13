using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerGroundCheck playerGroundCheck;
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

    public bool isWalking = false;
    public bool isSprinting = false;

    private void Update()
    {
        movementInput = movementAction.action.ReadValue<Vector3>();
    }

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerGroundCheck = GetComponent<PlayerGroundCheck>();

        cameraTranform = Camera.main.transform;
    }

    public void HandleMovementDirection()
    {
        movementDir = new Vector3(movementInput.x, 0f, movementInput.z);
        movementDir = cameraTranform.forward * movementDir.z + cameraTranform.right * movementDir.x;
        movementDir.y = 0f;

        movementDir.Normalize();
    }

    public void HandlePlayerMovement()
    {
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

            if(playerHorizontalVelocity.magnitude > maxSpeed)
            {
                float excessSpeed = playerHorizontalVelocity.magnitude - maxSpeed;

                Vector3 breakForce = -playerHorizontalVelocity.normalized * excessSpeed;
                playerRigidBody.AddForce(breakForce/2, ForceMode.Acceleration);
            }
        }
        
        isWalking = movementInput.magnitude > 0.1f;
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

    private void OnEnable()
    {
        jumpAction.action.started += Jump;

        sprintAction.action.started += SprintStarting;
        sprintAction.action.canceled += SprintEnding;
    }

    private void OnDisable()
    {
        jumpAction.action.started -= Jump;

        sprintAction.action.started -= SprintStarting;
        sprintAction.action.canceled -= SprintEnding;
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if(playerGroundCheck.isGrounded)
            playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void SprintStarting(InputAction.CallbackContext obj)
    {
        isSprinting = true;
    }

    private void SprintEnding(InputAction.CallbackContext obj)
    {
        isSprinting = false;
    }
}