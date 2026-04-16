using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;

    private GameObject[] allSpawnPoints;

    void Start()
    {
        // Find all points once at the start to save performance
        allSpawnPoints = GameObject.FindGameObjectsWithTag("Enemy Spawn");

        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        if (allSpawnPoints.Length > 0)
        {
            Transform selectedPoint = allSpawnPoints[0].transform;
            Instantiate(enemyPrefab, selectedPoint.position, selectedPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Player is too far/too close to all spawn points!");
        }
    }
}