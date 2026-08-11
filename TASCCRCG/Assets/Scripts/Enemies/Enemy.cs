using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Enemy Fields
    private float damageFlashDuration = 0.1f;
    private SpriteRenderer spriteRenderer;
    protected EnemyLootManager lootManager;
    protected ScoreManager scoreManager;
    protected DifficultyManager difficultyManager;
    protected bool isFlashing = false;
    protected int health;
    protected int scoreValue;

    // Enemy count helper fields
    public event System.Action<Enemy> OnEnemyRemoved;
    private bool isRemoved = false;

    protected virtual void Awake()
    {
        lootManager = GameObject.FindAnyObjectByType<EnemyLootManager>();
        scoreManager = GameObject.FindAnyObjectByType<ScoreManager>();
        difficultyManager = FindAnyObjectByType<DifficultyManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(lootManager == null)
        {
            Debug.LogError($"{name}: LootManager was not found.", this);
        }

        if (scoreManager == null)
        {
            Debug.LogError($"{name}: ScoreManager was not found.", this);
        }

        if (difficultyManager == null)
        {
            Debug.LogError($"{name}: DifficultyManager was not found.", this);
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    // Behaviour when the enemy is hit
    protected virtual void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            OnDestroyed();
        }
        else
        {
            // avoids flashing multiple time at the same time
            if (!isFlashing)
            {
                StartCoroutine(DamageFlashCoroutine());
            }
        }
    }

    // Behaviour when the enemy is destroyed
    protected virtual void OnDestroyed()
    {
        RemoveEnemy(true);
    }

    // Expand OnDestroyed to include enemy tracking
    private void RemoveEnemy(bool dropableLoot)
    {
        if (isRemoved)
        {
            return;
        }

        isRemoved = true;

        OnEnemyRemoved?.Invoke(this); // Run all subscribed methods on this enemy

        if( dropableLoot)
        {
            lootManager.EnemyDown(gameObject, scoreValue);
        }

        Destroy(gameObject);
    }

    private IEnumerator DamageFlashCoroutine()
    {
        isFlashing = true;

        // Enemy flashes quickly when taking damage.
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.enabled = true;

        isFlashing = false;

    }

    // Handling collision to avoid OnCollisionEnter2D and OnTriggerEnter2D to repeat code.
    private void HandleCollision(GameObject otherObject)
    {
        if (otherObject.CompareTag("EnemyWall"))
        {
            RemoveEnemy(false); // No drops when removed by wall
            return;
        }
        if (otherObject.CompareTag("PlayerBullet"))
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    // Functions to spawn different types of projectiles based on enum
    protected EnemyBullet SpawnProjectile(ProjectileType type, Vector3 direction, float angle)
    {
        // Enemy will pick from saw pool to spawn a saw
        EnemyBullet bullet = ProjectilePoolManager.Instance.GetProjectile(type);

        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        bullet.Initialize(direction);

        return bullet;
    }

}
