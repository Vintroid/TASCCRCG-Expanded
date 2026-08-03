using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wrench : MonoBehaviour
{
    // We want access to the GameManager functions from this class.
    [SerializeField] GameManager gameManager;

    // Enemy body
    [SerializeField] Rigidbody2D rb;

    // Enemy global fields
    private int health = 2;
    private float damageFlashDuration = 0.1f;
    private SpriteRenderer spriteRenderer;
    private bool isFlashing = false;

    // Enemy unique fields
    [SerializeField] int baseHealth = 2;
    [SerializeField] int scoreValue = 50;
    [SerializeField] float speed = 2f;



    void Awake()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        health = baseHealth + gameManager.difficulty;
    }

    void Update()
    {
        // Enemy movement
        transform.position += Vector3.left * Time.deltaTime * speed;
    }

    // Behaviour when the enemy is hit
    private void TakeDamage()
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

    IEnumerator DamageFlashCoroutine()
    {
        isFlashing = true;

        // Enemy flashes quickly when taking damage.
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.enabled = true;

        isFlashing = false;


    }

    // Behaviour when the enemy is destroyed
    private void OnDestroyed()
    {
        gameManager.playerManager.AddScore(50);

        // Powerups and other effects when destroyed
        gameManager.EnemyDown(this.gameObject);
        GameObject.Destroy(this.gameObject);
    }

    // Handling collision to avoid OnCollisionEnter2D and OnTriggerEnter2D to repeat code.
    private void HandleCollision(GameObject gameObject)
    {
        if (gameObject.CompareTag("EnemyWall"))
        {
            Destroy(gameObject);
        }
        if (gameObject.CompareTag("PlayerBullet"))
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }
}
