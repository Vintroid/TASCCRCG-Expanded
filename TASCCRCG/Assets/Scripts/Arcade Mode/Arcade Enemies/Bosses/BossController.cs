using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isFlashing = false;
    [SerializeField] private float damageFlashDuration = 0.1f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 30;

    protected BossState currentState;
    
    public int CurrentHealth { get; private set; }
    public bool IsDefeated { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (IsDefeated)
        {
            return;
        }

        currentState?.Update();
    }

    // Boss fight initialization
    public void StartFight()
    {
        CurrentHealth = maxHealth;
        IsDefeated = false;

        Debug.Log($"Boss fight started! HP:{CurrentHealth}");
    }

    public void TakeDamage(int damage)
    {
        if (IsDefeated)
        {
            return;
        }

        CurrentHealth -= damage;

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            DefeatBoss();
        }

        if (!isFlashing)
        {
            StartCoroutine(DamageFlashCoroutine());
        }
    }

    // Boss beaten cleanup
    public void DefeatBoss()
    {
        if (IsDefeated)
        {
            return;
        }

        IsDefeated = true;

        Debug.Log("Boss Defeated");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void HandleCollision(GameObject otherObject)
    {
        if (otherObject.CompareTag("PlayerBullet"))
        {
            TakeDamage(1);
        }
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

    // Boss can change states (patterns, behaviour, etc.)
    private void ChangeState(BossState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
