using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody enemyRigidBody;

    public bool isWalking;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;

    private void Start()
    {
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
        float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) *Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

        enemyRigidBody.rotation = Quaternion.Slerp(enemyRigidBody.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }    
}