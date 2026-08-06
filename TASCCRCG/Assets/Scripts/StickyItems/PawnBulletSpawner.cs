using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PawnBulletSpawner : MonoBehaviour
{
    [Header("Bullet Fields")]
    [SerializeField] private GameObject diagonalPlayerBullet;
    [SerializeField] private float bulletTime = 2f;
    [SerializeField] private float bulletCooldownTime = 0.25f;

    [Header("Pawn State")]
    [SerializeField] public bool attachedToPlayer = false;

    private Player player;
    private float bulletCooldownTimer;

    protected virtual void Awake()
    {
        // Pawns are attached to player object
        player = GetComponentInParent<Player>();
    }

    protected virtual void Update()
    {
        if (!attachedToPlayer)
        {
            return;
        }
        
        if(player == null)
        {
            player = GetComponentInParent<Player>();

            if(player == null)
            {
                return;
            }
        }

        UpdateTimers();

        if (player.IsShooting && bulletCooldownTimer <= 0f)
        {
            bulletCooldownTimer = bulletCooldownTime;
            Shoot();
        }
        
    }

    private void UpdateTimers()
    {
        if (bulletCooldownTimer > 0)
        {
            bulletCooldownTimer -= Time.deltaTime;
        }
    }

    protected abstract void Shoot();

    protected void SpawnBullet(Vector3 direction, float angle)
    {
        GameObject bulletObject = Instantiate(diagonalPlayerBullet, transform.position, Quaternion.Euler(0, 0, angle));

        PlayerBullet bullet = bulletObject.GetComponent<PlayerBullet>();

        if(bullet == null)
        {
            Debug.LogError($"{name}: Bullet prefab has no PlayerBullet component.");

            Destroy(bulletObject);
            return;
        }

        bullet.Initialize(direction);
        Destroy(bulletObject, bulletTime); // Set the bullet to die after a set time.
    }

    public void AttachToPlayer(Player attachedPlayer)
    {
        player = attachedPlayer;
        attachedToPlayer = true;
    }

    public void DetachFromPlayer()
    {
        player = null;
        attachedToPlayer = false;
    }


}
