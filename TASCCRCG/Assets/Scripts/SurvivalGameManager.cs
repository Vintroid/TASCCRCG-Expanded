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
    public int powerupRate = 2;
    public int weaponRate = 2;
    public int waveCounter = 0;
    public float powerupTimer = 3f;

    [Header("Managers")]
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] private WaveSpawner waveSpawner;

    [Header("Prefabs")]
    [SerializeField] GameObject bishopPowerup;
    [SerializeField] GameObject rookPowerup;
    [SerializeField] GameObject queenPowerup;
    [SerializeField] GameObject pawnUp;
    [SerializeField] GameObject pawnDown;

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
    public void EnemyDown(GameObject enemy)
    {
        // Enemy transform
        Vector3 enemyPos = enemy.transform.position;
        Quaternion enemyQuat = enemy.transform.rotation;

        // Enemies can drop weapons
        int randomIntWpn = UnityEngine.Random.Range(1, 11);

        // Weapons roll first
        if (randomIntWpn <= weaponRate)
        {
            int rng = UnityEngine.Random.Range(1, 101);

            // Up pawn weapon
            if(rng <= 51)
            {
                GameObject pUp = GameObject.Instantiate(pawnUp, enemyPos, enemyQuat);
            }
            else
            {
                GameObject pDown = GameObject.Instantiate(pawnDown, enemyPos, enemyQuat);
            }
        }

        // Enemies can spawn powerups
        else
        {
            int randomIntPwrUp = UnityEngine.Random.Range(1, 11);

            // Powerup roll 
            if (randomIntPwrUp <= powerupRate)
            {
                // Making the queen powerup rarer
                int rng = UnityEngine.Random.Range(1, 101);

                // Rook powerup
                if(rng <= 45)
                {
                    GameObject powerup = GameObject.Instantiate(rookPowerup, enemyPos, enemyQuat);
                    Destroy(powerup, powerupTimer);
                }

                // Bishop powerup
                if(rng <= 90 && rng > 45)
                {
                    GameObject powerup = GameObject.Instantiate(bishopPowerup, enemyPos, enemyQuat);
                    Destroy(powerup, powerupTimer);
                }

                // Queen powerup
                if(rng > 90)
                {
                    GameObject powerup = GameObject.Instantiate(queenPowerup, enemyPos, enemyQuat);
                    Destroy(powerup, powerupTimer);
                }
            }

        }
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
