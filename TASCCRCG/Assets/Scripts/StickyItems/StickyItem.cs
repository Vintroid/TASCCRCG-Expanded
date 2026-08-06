using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StickyItem : MonoBehaviour
{
    public bool stuckToPlayer { get; private set; }
    private PawnBulletSpawner bulletSpawner;

    void Awake()
    {
        bulletSpawner = GetComponent<PawnBulletSpawner>();

        if(bulletSpawner == null)
        {
            Debug.LogError($"{name}: StickyItem requires a PawnBulletSpawner component.");
        }
    }

    // Keep BOTH OnTriggerEnter2D and OnCollisionEnter2D

    // assuming StickyItems are NOT triggers but that enemies and enemy bullets are
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleInteraction(collision.gameObject);

    }

    // assuming StickyItems are NOT triggers but that enemies and enemy bullets are
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleInteraction(collision.gameObject);
    }

    private void HandleInteraction(GameObject other)
    {
        if (!stuckToPlayer)
        {
            TryAttach(other);
            return;
        }

        if (IsDamageSource(other))
        {
            Destroy(gameObject);
        }
    }

    private void TryAttach(GameObject other)
    {
        Player player = other.GetComponent<Player>();

        // Pawn stuck to player
        if(player != null)
        {
            Attach(player, player.transform);
            return;
        }

        StickyItem otherStickyItem = other.GetComponent<StickyItem>();

        if(otherStickyItem == null || !otherStickyItem.stuckToPlayer)
        {
            return;
        }

        player = otherStickyItem.GetComponentInParent<Player>();

        if(player == null)
        {
            return;
        }

        // Pawn stuck to another sticky item
        Attach(player, otherStickyItem.transform);
    }

    private void Attach(Player player, Transform newParent)
    {
        transform.SetParent(newParent, true);
        stuckToPlayer = true;

        bulletSpawner.AttachToPlayer(player);
    }

    // What can hurt the sticky items?
    private static bool IsDamageSource(GameObject other)
    {
        return other.CompareTag("EnemyBullet") || other.GetComponent<Enemy>() != null;
    }
}