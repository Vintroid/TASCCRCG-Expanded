using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    // Fields
    [Header("Prefabs")]
    [SerializeField] public GameObject bishopHat;
    [SerializeField] public GameObject rookHat;
    [SerializeField] public GameObject queenHat;

    [Header("Movement")]
    [SerializeField] public float moveSpeed = 5.0f;
    [SerializeField] public float scrollSpeed = 1.5f;
    [SerializeField] public float xMin;
    [SerializeField] public float xMax;
    [SerializeField] public float yMin;
    [SerializeField] public float yMax;

    [Header("Damage")]
    [SerializeField] private float damageCooldownTime = 2f;
    [SerializeField] private float damageFlashDuration = 0.1f;
    private float damageCooldownTimer;

    protected playerManager playerManager;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    public bool IsShooting { get; private set; }

    protected virtual void Awake()
    {
        // playerManager script reference
        playerManager = FindAnyObjectByType<playerManager>();
        if (playerManager == null)
        {
            Debug.LogError($"{name}: playerManager was not found.");
        }

        playerManager.OnModeChanged.AddListener(HandleModeChanged);

        spriteRenderer = GetComponent<SpriteRenderer>();

        rb = this.gameObject.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    protected virtual void Start()
    {
        HandleModeChanged();
    }

    protected virtual void Update()
    {
        if (playerManager != null && playerManager.gameOver)
        {
            this.gameObject.SetActive(false);
        }
        UpdateDamageCooldown();
        UpdateMovement();
        IsShooting = ReadShootInput();
    }

    private void HandleModeChanged()
    {
        if (playerManager == null)
        {
            return;
        }

        // Can setActive hats comparing playerManager mode
        bishopHat.SetActive(playerManager.mode == "bishop");
        rookHat.SetActive(playerManager.mode == "rook");
        queenHat.SetActive(playerManager.mode == "queen");

    }

    // Invincibility after damage
    private void UpdateDamageCooldown()
    {
        if (damageCooldownTimer > 0)
        {
            damageCooldownTimer -= Time.deltaTime;
        }
    }

    private void UpdateMovement()
    {
        Vector2 input = ReadMovementInput();
        Vector3 pos = transform.position;
        pos.x += input.x * moveSpeed * Time.deltaTime;
        pos.y += input.y * moveSpeed * Time.deltaTime;

        pos.x -= scrollSpeed * Time.deltaTime; // Scrolling drift

        // Clamp the position values inside player box.
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);

        transform.position = pos;
    }

    protected abstract Vector2 ReadMovementInput();
    protected abstract bool ReadShootInput();
    protected abstract void ReduceSharedHealth();

    // Keep BOTH OnTriggerEnter2D and OnCollisionEnter2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleInteraction(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        HandleInteraction(other.gameObject);

    }

    private void HandleInteraction(GameObject other)
    {
        // hat collisions
        if (other.CompareTag("bishopPowerUp"))
        {
            CollectPowerUp(other, "bishop");
        }
        if (other.CompareTag("rookPowerUp"))
        {
            CollectPowerUp(other, "rook");
        }
        if (other.CompareTag("queenPowerUp"))
        {
            CollectPowerUp(other, "queen");
        }
        // enemy collisions (take damage)
        if (IsDamageSource(other) && damageCooldownTimer <= 0)
        {
            Debug.Log("collided with: " + other.gameObject);
            StartCoroutine(OnHitRoutine());
        }
       
    }

    private void CollectPowerUp(GameObject powerUp, string mode)
    {
        playerManager.ChangeMode(mode);
        playerManager.AddScore(10);
        Destroy(powerUp);
    }

    // Needs to scale with new with tags that can hurt player
    private static bool IsDamageSource(GameObject other)
    {
        return other.CompareTag("EnemyBullet") || other.GetComponent<Enemy>();
    }

    // blink when hit
    private IEnumerator OnHitRoutine()
    {
        damageCooldownTimer = damageCooldownTime;
        ReduceSharedHealth();

        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.enabled = true;
    }

    protected IEnumerator BlinkRedRoutine()
    {
        const int blinkCount = 3;
        Color originalColor = spriteRenderer.color;
        Color redColor = new Color(1f, 162f / 255f, 162f / 255f); // FFA2A2

        for (int i = 0; i < blinkCount; i++)
        {
            spriteRenderer.color = redColor; // FFA2A2
            yield return new WaitForSeconds(damageFlashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(damageFlashDuration);
        }
    }
}
