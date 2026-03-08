using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float life = 3;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float bulletPushbackMeasure;
    private Vector3 pushBackDirection;
    private bool damageIsDealt = false;
    private void Awake()
    {
        Destroy(gameObject, life);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damageIsDealt)
            return;
        if (other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            pushBackDirection = (other.transform.position - transform.position).normalized;

            GameObject player = other.gameObject;
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            Rigidbody playerRigidBody = player.GetComponentInParent<Rigidbody>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(bulletDamage);
                damageIsDealt = true;
            }

            if (playerRigidBody != null)
                playerRigidBody.AddForce(pushBackDirection * bulletPushbackMeasure, ForceMode.Impulse);
        }

        Destroy(gameObject);
        damageIsDealt = false;
    }
}
