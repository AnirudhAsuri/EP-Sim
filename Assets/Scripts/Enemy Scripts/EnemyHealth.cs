using UnityEngine;

public class EnemyHealth : Health
{
    private EnemyMovement enemyMovement;
    private EnemyAIManager enemyAIManager;

    public float pushBackMeasure;
    public Vector3 pushBackDirection;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAIManager = GetComponent<EnemyAIManager>();

        InitialiseTotalHealth();
    }

    override
        public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
