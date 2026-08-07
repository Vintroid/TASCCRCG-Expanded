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
    [SerializeField] private float spawnX = 10f;
    [SerializeField] private float fixedY = 0f;
    [SerializeField] private float minY = -2f;
    [SerializeField] private float maxY = 3f;

    public GameObject EnemyPrefab => enemyPrefab;
    public int Amount => amount;
    public float SpawnInterval => spawnInterval;
    public SpawnPositionMode SpawnPositionMode => spawnPositionMode;

    public float SpawnX => spawnX;
    public float FixedY => fixedY;
    public float MinY => minY;
    public float MaxY => maxY;
}
