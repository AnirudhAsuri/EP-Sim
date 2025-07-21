using UnityEngine;

public class EnemyHealth : Health
{
    public float pushBackMeasure;
    public Vector3 pushBackDirection;

    private void Start()
    {
        InitialiseTotalHealth();
    }

    override
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
        }
}
