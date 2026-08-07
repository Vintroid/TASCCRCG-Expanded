using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnEntry
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int amount = 1;
    [SerializeField] private float spawnInterval = 0.5f;

    public GameObject EnemyPrefab => enemyPrefab;
    public int Amount => amount;
    public float SpawnInterval => spawnInterval;
}
