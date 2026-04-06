using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    [SerializeField] private GameObject giantPowerUp;
    [SerializeField] private GameObject healthRegenPowerUp;
    [SerializeField] private GameObject icePowerUp;
    [SerializeField] private GameObject sandPowerUp;
    [SerializeField] private GameObject invulnerabilityPowerUp;
    [SerializeField] private GameObject speedPowerUp;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void RevealPowerUp(Transform spawnTransform, float nothingProb, float giantProb, float healthRegenProb,
                          float iceProb, float sandProb, float invulnProb, float speedProb)
    {
        float totalWeight = nothingProb + giantProb + healthRegenProb + iceProb + sandProb + invulnProb + speedProb;

        float randomValue = Random.Range(0, totalWeight);

        if (randomValue < nothingProb)
        {
            Debug.Log("No power-up dropped this time.");
            return;
        }

        float currentThreshold = nothingProb;

        if (randomValue < (currentThreshold += giantProb)) { Spawn(giantPowerUp, spawnTransform); }
        else if (randomValue < (currentThreshold += healthRegenProb)) { Spawn(healthRegenPowerUp, spawnTransform); }
        else if (randomValue < (currentThreshold += iceProb)) { Spawn(icePowerUp, spawnTransform); }
        else if (randomValue < (currentThreshold += sandProb)) { Spawn(sandPowerUp, spawnTransform); }
        else if (randomValue < (currentThreshold += invulnProb)) { Spawn(invulnerabilityPowerUp, spawnTransform); }
        else { Spawn(speedPowerUp, spawnTransform); }
    }

    private void Spawn(GameObject prefab, Transform spawnTransform)
    {
        if (prefab == null) return;

        Vector3 spawnPoint = new Vector3(spawnTransform.position.x, 1f, spawnTransform.position.z);

        Instantiate(prefab, spawnPoint, Quaternion.identity);
    }
}