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

    private VictorySnapshot victorySnapshot;

    private void Start()
    {
        enemyCountText = GetComponentInChildren<TextMeshProUGUI>();
        victorySnapshot = FindObjectOfType<VictorySnapshot>();
    }

    private void Update()
    {
        enemyCount = EnemyHealth.AllEnemies.Count;
        HandleEnemyCountDisplay();

        if(enemyCount == 0)
        {
            if (victorySnapshot == null)
                victorySnapshot = FindObjectOfType<VictorySnapshot>();

            victorySnapshot.TakeVictoryPicture();
            OnAllEnemiesDefeated?.Invoke();
        }
    }

    public void HandleEnemyCountDisplay()
    { 
        enemyCountText.text = "Enemies Remaining : " + enemyCount;
    }
}
