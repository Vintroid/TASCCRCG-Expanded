using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StickyItem : MonoBehaviour
{
    public bool stuckToPlayer { get; private set; }
    private PawnBulletSpawner bulletSpawner;
    private Player parentPlayer;
    private ScoreManager scoreManager;

    private void Awake()
    {
        bulletSpawner = GetComponent<PawnBulletSpawner>();
        scoreManager = FindAnyObjectByType<ScoreManager>();

        if (!TryGetComponent(out bulletSpawner))
        {
            Debug.LogError($"{name}: StickyItem requires a PawnBulletSpawner component.", this);
            enabled = false;
        }

        if (scoreManager == null)
        {
            Debug.LogError($"{name}: ScoreManager was not found.", this);
            enabled = false;
            return;
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
        // Pawns attach to player
        if (!stuckToPlayer)
        {
            TryAttach(other);
            return;
        }

        // Pawns can collect powerups
        if(parentPlayer != null && parentPlayer.TryCollectPowerUp(other))
        {
            return;
        }

        // Pawns can disappear when hurt by objects
        if (IsDamageSource(other))
        {
            Destroy(gameObject);
        }
    }

    private void TryAttach(GameObject other)
    {
        // Checking if sticky item to attach
        StickyItem otherStickyItem = other.GetComponentInParent<StickyItem>();

        if (otherStickyItem != null) // Found a stickyitem to attach to
        {
            if (otherStickyItem == this) // Ignore own stickyItem object
            {
                return;
            }

            if (!otherStickyItem.stuckToPlayer) // Ignore items unattached
            {
                return;
            }


            Player parentPlayer = otherStickyItem.GetComponentInParent<Player>();

            if (parentPlayer == null)
            {
                Debug.LogWarning(
                    $"{otherStickyItem.name} is marked as attached, " +
                    "but has no Player in its parent hierarchy.");

                return;
            }

            Attach(parentPlayer, otherStickyItem.transform); // Attach stickyitem to stickyitem
            scoreManager.AddScore(10); // More points when chaining pawns

            return;

        }

        // Pawn stuck to human directly check
        Player player = other.GetComponentInParent<Player>();

        if(player != null)
        {
            Attach(player, player.transform);
            scoreManager.AddScore(5);
        }
        
    }

    private void Attach(Player player, Transform newParent)
    {
        transform.SetParent(newParent, true);
        stuckToPlayer = true;
        parentPlayer = player; // Useful for pawns to reference player. Can pick powerups!
        bulletSpawner.AttachToPlayer(player);
    }

    // What can hurt the sticky items?
    private static bool IsDamageSource(GameObject other)
    {
        return other.GetComponentInParent<EnemyBullet>() != null || other.GetComponentInParent<Enemy>() != null;
    }
}