using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeLevelManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private WaveSpawner waveSpawner;

    [Header("Level")]
    [SerializeField] private ArcadeLevelDefinition levelDefinition;
    private int currentWaveIndex = 0;
    private bool levelStarted = false;

    public bool IsLevelComplete { get; private set; }
    public event System.Action OnLevelCompleted;

    void Start()
    {
        StartLevel(levelDefinition);
    }

    // Starting Level Definitions
    public void StartLevel(ArcadeLevelDefinition newLevel)
    {
        if(newLevel == null)
        {
            Debug.LogError($"{name}: Cannot start a null level definition.", this);
            return;
        }

        levelDefinition = newLevel;
        currentWaveIndex = 0;
        IsLevelComplete = false;

        StartCoroutine(RunLevel());
    }

    // Running predetermined waves forming a level.
    private IEnumerator RunLevel()
    {
        levelStarted = true;

        while(currentWaveIndex < levelDefinition.Waves.Length)
        {
            WaveDefinition wave = levelDefinition.Waves[currentWaveIndex];

            if(wave == null)
            {
                Debug.LogWarning($"{name}: Wave {currentWaveIndex} is null.", this);
                
                currentWaveIndex++;
                continue;
            }

            playerManager.SetWave(currentWaveIndex + 1);
            waveSpawner.StartWave(wave);

            // Stop while loop here when enemies are left
            while (!waveSpawner.IsWaveComplete)
            {
                yield return null;
            }

            currentWaveIndex++;

            if(currentWaveIndex < levelDefinition.Waves.Length)
            {
                yield return new WaitForSeconds(levelDefinition.TimeBetweenWaves);
            }
        }

        LevelComplete();
    }

    private void LevelComplete()
    {
        IsLevelComplete = true;

        Debug.Log("Arcade Level Complete!");

        OnLevelCompleted?.Invoke();
    }
}
