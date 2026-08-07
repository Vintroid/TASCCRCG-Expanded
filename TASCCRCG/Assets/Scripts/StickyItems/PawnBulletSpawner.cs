using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PawnBulletSpawner : MonoBehaviour
{
    [Header("Bullet Fields")]
    [SerializeField] private float bulletCooldownTime = 0.25f;
    private float bulletCooldownTimer;


    [Header("Pawn State")]
    [SerializeField] private bool attachedToPlayer = false;

    private Player player;
    [SerializeField] private PlayerBulletPool bulletPool;

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
        if (bulletPool == null)
        {
            return;
        }

        bulletPool.SpawnBullet(PlayerBulletType.Diagonal,transform.position,direction,angle);
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
