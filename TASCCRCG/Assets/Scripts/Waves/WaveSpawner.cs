using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public bool IsSpawning {  get; private set; }

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

            for (int i = 0; i < entry.Amount; i++) {

                Instantiate(entry.EnemyPrefab, GetSpawnPosition(entry), Quaternion.identity);

                yield return new WaitForSeconds(entry.SpawnInterval);
            }
        }
        IsSpawning = false;
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
