using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLootManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject bishopPowerup;
    [SerializeField] GameObject rookPowerup;
    [SerializeField] GameObject queenPowerup;
    [SerializeField] GameObject pawnUp;
    [SerializeField] GameObject pawnDown;

    [Header("Drop Settings")]
    [SerializeField] private int powerupRate = 2;
    [SerializeField] private int weaponRate = 2;
    [SerializeField] private float powerupTimer = 3f;

    [SerializeField] private ScoreManager scoreManager;
   
    public void EnemyDown(GameObject enemy, int scoreValue)
    {
        scoreManager.AddScore(scoreValue);

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
            if (rng <= 51)
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
                if (rng <= 45)
                {
                    GameObject powerup = GameObject.Instantiate(rookPowerup, enemyPos, enemyQuat);
                    Destroy(powerup, powerupTimer);
                }

                // Bishop powerup
                if (rng <= 90 && rng > 45)
                {
                    GameObject powerup = GameObject.Instantiate(bishopPowerup, enemyPos, enemyQuat);
                    Destroy(powerup, powerupTimer);
                }

                // Queen powerup
                if (rng > 90)
                {
                    GameObject powerup = GameObject.Instantiate(queenPowerup, enemyPos, enemyQuat);
                    Destroy(powerup, powerupTimer);
                }
            }

        }
    }
}
