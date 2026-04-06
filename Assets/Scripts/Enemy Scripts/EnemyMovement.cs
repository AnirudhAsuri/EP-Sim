using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyManager enemyManager;
    private EnemyRangedAttacking enemyRangedAttacking;
    private EnemyAIManager enemyAIManager;
    private EnemyFatigue enemyFatigue;
    public Rigidbody enemyRigidBody;
    private TargetDetectionSystem targetDetectionSystem;

    public bool isWalking;

    public float movementSpeed;
    public float maxSpeed;
    public float rotationSpeed;

    [SerializeField] private float grassMovementSpeed;
    [SerializeField] private float sandMovementSpeed;
    [SerializeField] private float icyMovementSpeed;
    [SerializeField] private float defaultMovementSpeed;

    private float groundedCheckDistance;

    [SerializeField] private bool isJumper = false;
    [SerializeField] private float jumpPower;

    [SerializeField] private float jumpCooldown = 1.5f;
    private float lastJumpTime;

    private string sandyLayer = "Sandy Floor";
    private string grassyLayer = "Grassy Floor";
    private string icyLayer = "Icy Floor";

    private string playerLayer = "Player";
    private LayerMask floorLayers;

    private int grassLayerInt, sandLayerInt, iceLayerInt;
    private int playerLayerInt;

    private bool isGrounded = false;

    public enum FloorType
    {
        GrassyFloor,
        SandyFloor,
        IcyFloor,
        Air
    };

    [SerializeField] private FloorType currentFloorType;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyRangedAttacking = GetComponent<EnemyRangedAttacking>();
        enemyAIManager = GetComponent<EnemyAIManager>();
        enemyFatigue = GetComponent<EnemyFatigue>();
        targetDetectionSystem = GetComponentInChildren<TargetDetectionSystem>();
        enemyRigidBody = GetComponent<Rigidbody>();

        floorLayers = (1 << grassLayerInt) | (1 << sandLayerInt) | (1 << iceLayerInt);

        grassLayerInt = LayerMask.NameToLayer(grassyLayer);
        sandLayerInt = LayerMask.NameToLayer(sandyLayer);
        iceLayerInt = LayerMask.NameToLayer(icyLayer);
        playerLayerInt = LayerMask.NameToLayer(playerLayer);
    }

    private void HandleEnemyMovementSpeed()
    {
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();

        // Multiply the component height by the transform's Y scale
        float worldHeight = capsule.height * transform.lossyScale.y;

        groundedCheckDistance = (worldHeight / 2f) + 0.01f;

        RaycastHit hit;

        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, groundedCheckDistance);

        if(isGrounded)
        {
            if (hit.transform.gameObject.layer == playerLayerInt)
            {
                enemyRigidBody.AddForce(transform.forward * 3f, ForceMode.Force);
            }

            if (hit.transform.gameObject.layer == grassLayerInt)
            {
                currentFloorType = FloorType.GrassyFloor;
            }

            else if (hit.transform.gameObject.layer == sandLayerInt)
            {
                currentFloorType = FloorType.SandyFloor;
            }

            else if (hit.transform.gameObject.layer == iceLayerInt)
            {
                currentFloorType = FloorType.IcyFloor;
            }
        }

        else
        {
            currentFloorType = FloorType.Air;
        }

        switch(currentFloorType)
        {
            case FloorType.GrassyFloor:
                movementSpeed = grassMovementSpeed;
                break;

            case FloorType.SandyFloor:
                movementSpeed = sandMovementSpeed;
                break;

            case FloorType.IcyFloor:
                movementSpeed = icyMovementSpeed;
                break;

            default:
                movementSpeed = defaultMovementSpeed;
                break;
        }
    }

    public void HandleEnemyMovement(Vector3 movementDirection)
    {
        HandleEnemyMovementSpeed();

        Vector3 enemyMovementDirection = movementDirection;
        if(gameObject.layer == enemyManager.rangedEnemyLayer)
        {
            if(enemyAIManager.distanceToTarget > enemyRangedAttacking.minimumMaintainedDistance)
            {
                enemyMovementDirection = movementDirection;
                isWalking = enemyRigidBody.velocity.magnitude > 0.1f;
            }
            else
            {
                enemyMovementDirection = -movementDirection;
                isWalking = false;
            }
        }

        Vector3 horizontalVelocity = new Vector3(enemyRigidBody.velocity.x, 0f, enemyRigidBody.velocity.z);

        if(currentFloorType == FloorType.SandyFloor && isJumper && isGrounded && Time.time >= lastJumpTime + jumpCooldown && !enemyFatigue.isInTiredState)
        {
            if(targetDetectionSystem.targetInVision)
            {
                Vector3 jumpDirection = (transform.up * 3f + transform.forward).normalized;

                enemyRigidBody.AddForce(jumpDirection * jumpPower, ForceMode.Impulse);

                lastJumpTime = Time.time;
            }
        }

        enemyRigidBody.AddForce(enemyMovementDirection * movementSpeed, ForceMode.Acceleration);

        if(horizontalVelocity.magnitude > maxSpeed)
        {
            float excessSpeed = horizontalVelocity.magnitude - maxSpeed;

            Vector3 breakForce = -horizontalVelocity.normalized * excessSpeed;

            enemyRigidBody.AddForce(breakForce, ForceMode.Acceleration);
        }

        if(gameObject.layer == enemyManager.meleeEnemyLayer)
            isWalking = enemyRigidBody.velocity.magnitude > 0.1f;
    }

    public void HandleEnemyTurning(Vector3 movementDirection)
    {
        if (targetDetectionSystem.isSearching)
            return;

        Vector3 directionToTarget = (targetDetectionSystem.currentTargetPosition - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        enemyRigidBody.rotation = Quaternion.Slerp(enemyRigidBody.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}