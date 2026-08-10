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
    [SerializeField] private WaveDefinition[] waves;
    [SerializeField] private float timeBetweenWaves = 3f;

    private int currentWaveIndex = 0;
    private bool levelStarted = false;

    void Start()
    {
        StartCoroutine(RunLevel());
    }

    // Running predetermined waves forming a level.
    private IEnumerator RunLevel()
    {
        levelStarted = true;

        while(currentWaveIndex < waves.Length)
        {
            WaveDefinition wave = waves[currentWaveIndex];

            if(wave == null)
            {
                Debug.LogWarning($"{name}: Wave {currentWaveIndex} is null.", this);
                
                currentWaveIndex++;
                continue;
            }

            playerManager.SetWave(currentWaveIndex + 1);
            waveSpawner.StartWave(wave);

            // Stop while loop here when spawning
            while (waveSpawner.IsSpawning)
            {
                yield return null;
            }

            currentWaveIndex++;

            if(currentWaveIndex < waves.Length)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        LevelComplete();
    }

    private void LevelComplete()
    {
        Debug.Log("Arcade Level Complete!");
    }
}
