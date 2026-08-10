using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnEntry // Spawner object
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int amount = 1;
    [SerializeField] private float spawnInterval = 0.5f;

    [Header("Spawn Position")]
    [SerializeField] private SpawnPositionMode spawnPositionMode;

    // Fixed coordinates for spawning
    [Header("Fixed Coordinates Default")]
    [SerializeField] private float spawnX = 10f;
    [SerializeField] private float fixedY = 0f;
    [SerializeField] private float minY = -2.15f;
    [SerializeField] private float maxY = 2.85f;

    [Header("Difficulty Scaling")]
    [SerializeField] private bool scaleAmountWithDifficulty;
    [SerializeField] private int tiersPerExtraEnemy = 2;
    [SerializeField] private int maxExtraEnemies = 3;

    public GameObject EnemyPrefab => enemyPrefab;
    public int Amount => amount;
    public float SpawnInterval => spawnInterval;
    public SpawnPositionMode SpawnPositionMode => spawnPositionMode;

    public float SpawnX => spawnX;
    public float FixedY => fixedY;
    public float MinY => minY;
    public float MaxY => maxY;

    // Enemy amount scaling
    public int GetAmount(int difficultyTier)
    {
        if(!scaleAmountWithDifficulty || tiersPerExtraEnemy <= 0)
        {
            return amount;
        }

        int bonusAmount = difficultyTier / tiersPerExtraEnemy;

        return amount + Mathf.Min(bonusAmount, maxExtraEnemies);
    }
}
