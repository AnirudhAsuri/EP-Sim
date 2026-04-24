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

    [SerializeField] private int totalWaveCount;
    private int remainingWaves;
    private int currentWave;

    [SerializeField] private List<EnemyManager> waveTwoEnemies;
    [SerializeField] private List<EnemyManager> waveThreeEnemies;

    private string waveNumberTag = "Wave Number";
    [SerializeField] private TextMeshProUGUI waveNumberText;

    [SerializeField] private float waveStartDelay;
    private bool waveStarting;

    private VictorySnapshot victorySnapshot;

    private void Start()
    {
        victorySnapshot = FindObjectOfType<VictorySnapshot>();

        remainingWaves = totalWaveCount;
        currentWave = 1;

        DisableEnemies(waveTwoEnemies);
        DisableEnemies(waveThreeEnemies);
    }

    private void Update()
    {
        enemyCount = EnemyHealth.AllEnemies.Count;
        HandleEnemyCountDisplay();
        HandleWaveLogic();

        if (remainingWaves == 0)
        {
            if (victorySnapshot == null)
                victorySnapshot = FindObjectOfType<VictorySnapshot>();

            victorySnapshot.TakeVictoryPicture();
            OnAllEnemiesDefeated?.Invoke();
        }
    }

    private void HandleWaveLogic()
    {
        if (enemyCount == 0)
        {
            if (remainingWaves > 0 && waveStarting == false)
            {
                waveStarting = true;

                remainingWaves--;
                currentWave++;
            }

            if (remainingWaves == 0)
                return;

            switch(currentWave)
            {
                case 2:
                    StartCoroutine(StartNewWave(waveTwoEnemies));
                    break;

                case 3:
                    StartCoroutine(StartNewWave(waveThreeEnemies));
                    break;

                default:
                    Debug.Log("Default Case Wave");
                    break;
            }
        }
    }

    private void DisableEnemies(List<EnemyManager> enemies)
    {
        foreach (EnemyManager enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    private void EnableEnemies(List<EnemyManager> enemies)
    {
        foreach(EnemyManager enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    private IEnumerator StartNewWave(List<EnemyManager> enemies)
    {
        HandleWaveNumberDisplay();
        waveNumberText.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveStartDelay);

        EnableEnemies(enemies);
        waveNumberText.gameObject.SetActive(false);

        waveStarting = false;
    }

    private void HandleWaveNumberDisplay()
    {
        waveNumberText.text = "WAVE : " + currentWave;
    }

    public void HandleEnemyCountDisplay()
    { 
        enemyCountText.text = "Enemies Remaining : " + enemyCount;
    }
}
