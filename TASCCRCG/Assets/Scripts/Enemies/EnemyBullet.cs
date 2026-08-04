using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Bullet fields
    private Vector3 direction;
    [SerializeField] float bulletSpeed = 3f;

    void Update()
    {
        transform.position += direction * bulletSpeed * Time.deltaTime;
    }

    public void Initialize(Vector3 direction)
    {
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
            Destroy(gameObject);
        }
    }


    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
