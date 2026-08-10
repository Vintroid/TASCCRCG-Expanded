using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class SurvivalGameManager : MonoBehaviour
{
    // Probability of enemies waves now uses weights
    [System.Serializable]
    private class WaveWeights 
    {
        [Min(0)] public int wrench;
        [Min(0)] public int gear;
        [Min(0)] public int wrenchGear;
        [Min(0)] public int bigGear;

        public int TotalWeight => wrench + gear + wrenchGear + bigGear;
    }
    [Header("Survival Fields")]
    [SerializeField] private float difficultyInterval = 15f;
    [SerializeField] private int maxDifficulty = 7;
    public int difficultyTier = 0;
    public float survivalTimer = 0f;
    public float waveTimer = 8f;
    public float waveRate = 9f;
    public int waveCounter = 0;

    [Header("Managers")]
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private DifficultyManager difficultyManager;

    [Header("Wave Definitions")]
    [SerializeField] WaveDefinition wrenchWave;
    [SerializeField] WaveDefinition gearWave;
    [SerializeField] WaveDefinition wrenchGearWave;
    [SerializeField] WaveDefinition bigGearWave;

    // Array of weights for wave probabilities.
    [Header("Wave Selection")]
    [SerializeField] private WaveWeights[] waveWeightsByTier = new WaveWeights[8];

    // Update is called once per frame
    void Update()
    {
        if(playerManager == null || playerManager.IsGameOver)
        {
            return;
        }

        UpdateDifficulty();
        UpdateWaveTimer();
    }
    
    // Difficulty based on timer.
    private void UpdateDifficulty()
    {
        survivalTimer += Time.deltaTime;

        difficultyTier = Mathf.Clamp(Mathf.FloorToInt(survivalTimer / difficultyInterval), 0, maxDifficulty);
        difficultyManager.SetDifficultyTier(difficultyTier); // For global access to difficulty

        waveRate = difficultyManager.WaveInterval;
    }

    // Wave change based on timer/difficulty
    private void UpdateWaveTimer()
    {
        waveTimer += Time.deltaTime;

        if(waveTimer < waveRate)
        {
            return;
        }

        waveTimer = 0f;
        StartNextWave();
    }

    // Currently based on random-number generator selection.
    private WaveDefinition SelectWave()
    {
        if(waveWeightsByTier == null || waveWeightsByTier.Length == 0)
        {
            Debug.LogWarning($"{name}: No Survival wave weights configured", this);
            return wrenchWave;
        }

        int tierIndex = Mathf.Clamp(difficultyTier, 0, waveWeightsByTier.Length - 1);

        WaveWeights weights = waveWeightsByTier[tierIndex];

        if(weights == null || weights.TotalWeight <= 0)
        {
            Debug.LogWarning($"{name}: Tier {tierIndex} has no valid wave weights.",this);
            return wrenchWave;
        }

        // Rng for waves, but take it depending of total weight.
        int roll = UnityEngine.Random.Range(0, weights.TotalWeight);

        // For each check we remove from the roll so that the check are independent.
        if(roll < weights.wrench)
        {
            return wrenchWave;
        }

        roll -= weights.wrench;

        if(roll < weights.gear)
        {
            return gearWave;
        }

        roll -= weights.gear;

        if(roll < weights.wrenchGear)
        {
            return wrenchGearWave;
        }

        return bigGearWave;


        

       
        return wrenchWave;
    }

    private void StartNextWave()
    {
        WaveDefinition wave = SelectWave();

        if(wave == null)
        {
            return;
        }

        waveCounter++;
        playerManager.SetWave(waveCounter);

        waveSpawner.StartWave(wave);
    }
}
