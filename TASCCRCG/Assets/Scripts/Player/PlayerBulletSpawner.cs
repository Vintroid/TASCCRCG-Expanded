using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBulletSpawner : MonoBehaviour
{
    private PlayerManager playerManager;
    private Player player;

    [Header("Bullet Prefabs")]
    [SerializeField] private GameObject straightPlayerBullet;
    [SerializeField] private GameObject diagonalPlayerBullet;

    [Header("Fields")]
    [SerializeField] private float bulletTime = 10.0f;
    [SerializeField] private float bulletCooldownTime = 0.25f;
    private float bulletCooldownTimer = 0.0f;


    private void Awake()
    {
        player = GetComponent<Player>();

        if(player == null)
        {
            Debug.LogError($"{name}: PlayerBulletSpawner requires a Player component on the same GameObject.");
        }
    }

    private void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();

        if(playerManager == null)
        {
            Debug.LogError($"{name}: playerManager was not found.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(player == null || playerManager == null)
        {
            return;
        }

        UpdateTimers();

        if (player.IsShooting && bulletCooldownTimer <= 0f)
        {
            Shoot();
        }
        
        
    }

    // Counts down cooldown for firing again
    void UpdateTimers()
    {
        if (bulletCooldownTimer > 0)
        {
            bulletCooldownTimer -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        bulletCooldownTimer = bulletCooldownTime;

        if (playerManager.Mode == PlayerMode.Basic)
        {
            SpawnBullet(straightPlayerBullet, Vector3.right, 0f);
        }
        if (playerManager.Mode == PlayerMode.Rook || playerManager.Mode == PlayerMode.Queen)
        {
            SpawnBullet(straightPlayerBullet, Vector3.right, 0f);
            SpawnBullet(straightPlayerBullet, Vector3.up, 90f);
            SpawnBullet(straightPlayerBullet, Vector3.left, 180f);
            SpawnBullet(straightPlayerBullet, Vector3.down, 270f);
        }
        if (playerManager.Mode == PlayerMode.Bishop || playerManager.Mode == PlayerMode.Queen)
        {
            SpawnBullet(diagonalPlayerBullet, (Vector3.right + Vector3.up).normalized, 0f);
            SpawnBullet(diagonalPlayerBullet, (Vector3.left + Vector3.up).normalized, 90f);
            SpawnBullet(diagonalPlayerBullet, (Vector3.left + Vector3.down).normalized, 180f);
            SpawnBullet(diagonalPlayerBullet, (Vector3.right + Vector3.down).normalized, 270f);
        }
    }

    private void SpawnBullet(GameObject bulletPrefab, Vector3 direction, float angle)
    {
        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));

        PlayerBullet bullet = bulletObject.GetComponent<PlayerBullet>();
        bullet.Initialize(direction);

        Destroy(bulletObject, bulletTime);
    }
}
