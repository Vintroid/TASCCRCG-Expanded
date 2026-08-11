using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public bool IsSpawning {  get; private set; }

    private DifficultyManager difficultyManager;

    // Enemy Tracking
    private int activeEnemies = 0;
    public bool IsWaveComplete => !IsSpawning && activeEnemies <= 0;

    private void Awake()
    {
        difficultyManager = FindAnyObjectByType<DifficultyManager>();

        if(difficultyManager == null)
        {
            Debug.LogError($"{name}: DifficultyManager was not found.", this);
        }
    }
    public void StartWave(WaveDefinition wave)
    {
        if(wave == null)
        {
            Debug.LogError($"{name}: Cannot spawn a null WaveDefinition.", this);
            return;
        }

        StartCoroutine(SpawnWave(wave));
    }

    // Spawning function. Was previously handled in GameManager and Wave classes.
    private IEnumerator SpawnWave(WaveDefinition wave)
    {
        IsSpawning = true;

        foreach( EnemySpawnEntry entry in wave.Enemies)
        {
            if(entry.EnemyPrefab == null)
            {
                Debug.LogWarning($"{name}: Wave contains empty enemy prefab.", this);
                continue;
            }

            // Enemy amount 
            int amount = entry.GetAmount(difficultyManager.CurrentDifficultyTier);

            for (int i = 0; i < amount; i++) {

                // Enemy Tracking while instantiating
                GameObject enemyObject = Instantiate(entry.EnemyPrefab, GetSpawnPosition(entry), Quaternion.identity);

                Enemy enemy = enemyObject.GetComponent<Enemy>();

                if(enemy != null)
                {
                    activeEnemies++;
                    enemy.OnEnemyRemoved += HandleEnemyRemoved;
                }
                else
                {
                    Debug.LogWarning($"{name}: Spawned entry prefab has no Enemy component.", enemyObject);
                }

                yield return new WaitForSeconds(entry.SpawnInterval);
            }
        }
        IsSpawning = false;
    }

    private void HandleEnemyRemoved(Enemy enemy)
    {
        enemy.OnEnemyRemoved -= HandleEnemyRemoved;

        activeEnemies = Mathf.Max(0, activeEnemies - 1);
    }

    // Getting initial stating position for Instantiation
    private Vector3 GetSpawnPosition(EnemySpawnEntry entry)
    {
        switch (entry.SpawnPositionMode)
        {
            case SpawnPositionMode.RandomY:
                return new Vector3(
                    entry.SpawnX,
                    Random.Range(entry.MinY, entry.MaxY),
                    0f
                );

            case SpawnPositionMode.Fixed:
            case SpawnPositionMode.Ground:

            default:
                return new Vector3(
                    entry.SpawnX,
                    entry.FixedY
                );

        }
    }
}
