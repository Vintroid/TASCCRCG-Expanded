using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Enemy Fields
    protected GameManager gameManager;
    private float damageFlashDuration = 0.1f;
    private SpriteRenderer spriteRenderer;
    protected bool isFlashing = false;
    protected int health;
    protected int scoreValue;

    protected virtual void Awake()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        gameManager.playerManager.AddScore(scoreValue);

        // Powerups and other effects when destroyed
        gameManager.EnemyDown(this.gameObject);
        GameObject.Destroy(this.gameObject);
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
            Destroy(gameObject);
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
        Debug.Assert(ProjectilePoolManager.Instance != null, "ProjectilePoolManager.Instance is null!");

        // Enemy will pick from saw pool to spawn a saw
        EnemyBullet bullet = ProjectilePoolManager.Instance.GetProjectile(type);

        Debug.Assert(bullet != null, $"Pool returned null for projectile type {type}");

        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        bullet.Initialize(direction);

        return bullet;
    }

}
