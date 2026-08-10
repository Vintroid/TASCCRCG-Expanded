using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class SurvivalGameManager : MonoBehaviour
{
    [Header("Survival Fields")]
    [SerializeField] private float difficultyInterval = 15f;
    [SerializeField] private int maxDifficulty = 7;
    public int difficulty = 0;
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

        difficulty = Mathf.Clamp(Mathf.FloorToInt(survivalTimer / difficultyInterval), 0, maxDifficulty);
        difficultyManager.SetDifficulty(difficulty); // For global access to difficulty

        waveRate = 2f + (maxDifficulty - difficulty);
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
        int rng = UnityEngine.Random.Range(0, 100);

        // Mixed wave
        if (rng < 3 * difficulty + 15)
        {
            return wrenchGearWave;
        }

        // Solo waves
        if (rng <= 3 * difficulty + 20)
        {
            return bigGearWave;
        }

        if (rng <= 5 * difficulty + 20)
        {
            return gearWave;
        }

       
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
