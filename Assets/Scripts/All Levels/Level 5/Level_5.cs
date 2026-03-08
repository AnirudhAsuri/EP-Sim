using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_5 : LevelMenu
{
    private EnemyCount enemyCount;
    [SerializeField] private GameObject winScreen;

    private void Start()
    {
        enemyCount = FindAnyObjectByType<EnemyCount>();

        winScreen.SetActive(false);
    }

    private void Update()
    {
        enemyCount.HandleEnemyCountDisplay();

        if(EnemyHealth.AllEnemies.Count == 0)
            HandleLevelEnd();
    }

    public void HandleLevelEnd()
    {
        winScreen.SetActive(true);
        //HandleLevelUnlocking();
    }
}