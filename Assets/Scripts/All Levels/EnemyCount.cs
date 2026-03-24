using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class EnemyCount : MonoBehaviour
{
    public static event Action OnAllEnemiesDefeated;

    [SerializeField] private TextMeshProUGUI enemyCountText;
    public int enemyCount;

    [SerializeField] private VictorySnapshot victorySnapshot;

    private void Start()
    {
        enemyCountText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        enemyCount = EnemyHealth.AllEnemies.Count;
        HandleEnemyCountDisplay();

        if(enemyCount == 0)
        {
            victorySnapshot.TakeVictoryPicture();
            OnAllEnemiesDefeated?.Invoke();
        }
    }

    public void HandleEnemyCountDisplay()
    { 
        enemyCountText.text = "Enemies Remaining : " + enemyCount;
    }
}
