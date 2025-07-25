using UnityEngine;

public class EnemyHealth : Health
{
    public float pushBackMeasure;
    public Vector3 pushBackDirection;

    private void Awake()
    {
        InitialiseTotalHealth();
    }

    public override void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
