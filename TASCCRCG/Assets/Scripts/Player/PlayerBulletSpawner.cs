using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBulletSpawner : MonoBehaviour
{
    private PlayerManager playerManager;
    private Player player;
    [SerializeField] private PlayerBulletPool bulletPool;

    [Header("Fields")]
    [SerializeField] private float bulletCooldownTime = 0.25f;
    private float bulletCooldownTimer = 0.0f;


    private void Awake()
    {
        player = GetComponent<Player>();

        if(player == null)
        {
            Debug.LogError($"{name}: PlayerBulletSpawner requires a Player component on the same GameObject.");
        }

        bulletPool = FindAnyObjectByType<PlayerBulletPool>();
        if(bulletPool == null)
        {
            Debug.LogError($"{name}: PlayerBulletPoool not assigned in editor",this);
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
            SpawnBullet(PlayerBulletType.Straight, Vector3.right, 0f);
        }
        if (playerManager.Mode == PlayerMode.Rook || playerManager.Mode == PlayerMode.Queen)
        {
            SpawnBullet(PlayerBulletType.Straight, Vector3.right, 0f);
            SpawnBullet(PlayerBulletType.Straight, Vector3.up, 90f);
            SpawnBullet(PlayerBulletType.Straight, Vector3.left, 180f);
            SpawnBullet(PlayerBulletType.Straight, Vector3.down, 270f);
        }
        if (playerManager.Mode == PlayerMode.Bishop || playerManager.Mode == PlayerMode.Queen)
        {
            SpawnBullet(PlayerBulletType.Diagonal, (Vector3.right + Vector3.up).normalized, 0f);
            SpawnBullet(PlayerBulletType.Diagonal, (Vector3.left + Vector3.up).normalized, 90f);
            SpawnBullet(PlayerBulletType.Diagonal, (Vector3.left + Vector3.down).normalized, 180f);
            SpawnBullet(PlayerBulletType.Diagonal, (Vector3.right + Vector3.down).normalized, 270f);
        }
    }

    private void SpawnBullet(PlayerBulletType bulletType, Vector3 direction, float angle)
    {
        if(bulletPool == null)
        {
            return;
        }

        bulletPool.SpawnBullet(bulletType,transform.position,direction,angle);
    }
}
