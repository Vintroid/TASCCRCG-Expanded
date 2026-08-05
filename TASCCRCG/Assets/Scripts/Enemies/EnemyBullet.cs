using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBullet : MonoBehaviour
{
    // Bullet fields
    private Vector3 direction;
    [SerializeField] float bulletSpeed = 3f;

    // Pooling fields
    private IObjectPool<EnemyBullet> pool;
    private bool hasBeenReleased;

    void Update()
    {
        transform.position += direction * bulletSpeed * Time.deltaTime;
    }

    // Initialize is used to reset an EnemyBullet
    public void Initialize(Vector3 direction)
    {
        hasBeenReleased = false;
        this.direction = direction.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    protected virtual void HandleCollision(GameObject otherObject)
    {
        if (otherObject.CompareTag("PlayerBullet") ||
               otherObject.CompareTag("Player"))
        {
            if (hasBeenReleased)
                return;

            hasBeenReleased = true;
            pool.Release(this);
        }
    }

    private void OnBecameInvisible()
    {
        if (hasBeenReleased) return;

        hasBeenReleased = true;
        pool.Release(this);
    }

    // Setting that reference to the pool. Called from outside.
    public void SetPool(IObjectPool<EnemyBullet> enemyBulletPool)
    {
        this.pool = enemyBulletPool;
    }
}
