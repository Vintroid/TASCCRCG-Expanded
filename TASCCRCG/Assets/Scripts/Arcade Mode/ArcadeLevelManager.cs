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

    [Header("Boss")]
    [SerializeField] private BossEncounterController bossEncounterController;

    [SerializeField] private ScrollingController scrollingController;

    public bool IsLevelComplete { get; private set; }
    public event System.Action OnLevelCompleted;

    public bool IsLevelRunning { get; private set; }

    public ArcadeLevelDefinition CurrentLevel => levelDefinition; 

    void Start()
    {
        StartLevel(levelDefinition);
    }

    // Starting Level Definitions
    public void StartLevel(ArcadeLevelDefinition newLevel)
    {
        if (IsLevelRunning)
        {
            return;
        }

        if(newLevel == null)
        {
            Debug.LogError($"{name}: Cannot start a null level definition.", this);
            return;
        }

        levelDefinition = newLevel;
        currentWaveIndex = 0;
        IsLevelComplete = false;
        IsLevelRunning = true;

        StartCoroutine(RunLevel());
    }

    // Running predetermined waves forming a level.
    private IEnumerator RunLevel()
    {
        levelStarted = true;

        // Screen start scrolling if not already
        if (scrollingController != null)
        {
            scrollingController.StartScrolling();
        }

        while (currentWaveIndex < levelDefinition.Waves.Length)
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

        bossEncounterController.StartEncounter(CurrentLevel.BossPrefab);

        // Waiting to get signal the boss fight is done
        while (!bossEncounterController.IsEncounterComplete)
        {
            yield return null;
        }

        LevelComplete();
    }

    private void LevelComplete()
    {
        IsLevelComplete = true;
        IsLevelRunning = false;

        Debug.Log("Arcade Level Complete!");

        OnLevelCompleted?.Invoke();
    }
}
