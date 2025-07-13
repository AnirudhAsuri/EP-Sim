using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private GameObject enemyDeadBody;
    private EnemyHealth enemyHealth;

    private Vector3 deathPushBackDirection;

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    public void SwitchBodies()
    {
        GameObject deadBody = Instantiate(enemyDeadBody, transform.position, transform.rotation);

        deathPushBackDirection = new Vector3(enemyHealth.pushBackDirection.x, 0.2f, enemyHealth.pushBackDirection.z);

        Rigidbody deadBodyRigidBody = deadBody.GetComponent<Rigidbody>();
        if (deadBodyRigidBody != null)
        {
            deadBodyRigidBody.AddForce(enemyHealth.pushBackMeasure/3 * deathPushBackDirection, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
