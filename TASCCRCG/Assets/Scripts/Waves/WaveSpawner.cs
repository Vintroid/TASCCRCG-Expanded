using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WaveSpawner : MonoBehaviour
{
    public bool isSpawning {  get; private set; }

    [SerializeField] private WaveDefinition testWave;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartWave(testWave);
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

    private IEnumerator SpawnWave(WaveDefinition wave)
    {
        isSpawning = true;

        foreach( EnemySpawnEntry entry in wave.Enemies)
        {
            if(entry.EnemyPrefab == null)
            {
                Debug.LogWarning($"{name}: Wave contains empty enemy prefab.", this);
                continue;
            }

            for (int i = 0; i < entry.Amount; i++) {

                Instantiate(entry.EnemyPrefab, GetSpawnPosition(), Quaternion.identity);

                yield return new WaitForSeconds(entry.SpawnInterval);
            }
        }
        isSpawning = false;
    }

    private Vector3 GetSpawnPosition()
    {
        return transform.position;
    }
}
