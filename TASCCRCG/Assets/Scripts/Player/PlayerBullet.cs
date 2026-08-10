using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 5.0f;
    [SerializeField] private float maxLifetime = 5f;
    private float lifetimeTimer;
    public Vector3 direction;
    private Rigidbody2D rb;

    public PlayerBulletType BulletType { get; private set; }

    private PlayerBulletPool playerBulletPool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lifetimeTimer -= Time.deltaTime;

        if( lifetimeTimer <= 0f)
        {
            ReturnToPool();
        }
    }

    // Keep BOTH OnTriggerEnter2D and OnCollisionEnter2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ReturnToPool();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReturnToPool();
    }

    private void OnBecameInvisible()
    {
        ReturnToPool();
    }

    // Bullet just spawned and is initialized
    public void Initialize(Vector3 direction, PlayerBulletPool pool, PlayerBulletType type)
    {
        this.direction = direction.normalized;
        playerBulletPool = pool;
        BulletType = type;
        lifetimeTimer = maxLifetime; // Automatic removal after timer

        rb.velocity = this.direction * bulletSpeed; // Initialize bullet speed
    }

    // Implemented pooling for bullets, send bullet to the pool object
    public void ReturnToPool()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        rb.velocity = Vector2.zero; // Reset bullet velocity

        if(playerBulletPool == null)
        {
            gameObject.SetActive(false);
            return;
        }

        playerBulletPool.ReturnBullet(this);
    }
       
}
