using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody enemyRigidBody;
    private TargetDetectionSystem targetDetectionSystem;

    public bool isWalking;

    public float movementSpeed;
    public float maxSpeed;
    public float rotationSpeed;

    private void Start()
    {
        targetDetectionSystem = GetComponentInChildren<TargetDetectionSystem>();
        enemyRigidBody = GetComponent<Rigidbody>();
    }

    public void HandleEnemyMovement(Vector3 movementDirection)
    {
        Vector3 horizontalVelocity = new Vector3(enemyRigidBody.velocity.x, 0f, enemyRigidBody.velocity.z);

        enemyRigidBody.AddForce(movementDirection * movementSpeed, ForceMode.Acceleration);

        if(horizontalVelocity.magnitude > maxSpeed)
        {
            float excessSpeed = horizontalVelocity.magnitude - maxSpeed;

            Vector3 breakForce = -horizontalVelocity.normalized * excessSpeed;

            enemyRigidBody.AddForce(breakForce, ForceMode.Acceleration);
        }

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