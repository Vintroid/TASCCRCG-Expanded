using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    private bool isFlashing = false;
    [SerializeField] private float damageFlashDuration = 0.1f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 30;

    public event System.Action<BossController> OnBossDefeated;

    protected int currentPhase = 1;
    public int CurrentPhase => currentPhase;

    protected BossState currentState;
    // For debugging
    public string CurrentStateName => currentState?.GetType().Name ?? "None";
    
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
    public virtual void StartFight()
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
            return;
        }

        if (!isFlashing)
        {
            StartCoroutine(DamageFlashCoroutine());
        }

        OnHealthChanged();
    }

    // Can be expanded to account for phase changes for example.
    protected virtual void OnHealthChanged()
    {

    }

    // Bosses expand on phase change behaviour
    protected virtual void OnPhaseChanged()
    {

    }


    // Boss beaten cleanup
    public void DefeatBoss()
    {
        if (IsDefeated)
        {
            return;
        }

        IsDefeated = true;

        currentState?.Exit();
        currentState = null;

        Debug.Log("Boss Defeated");

        OnBossDefeated?.Invoke(this);

        HandleDefeat();
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
    protected void ChangeState(BossState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();

        Debug.Log($"{name}: Current State = {CurrentStateName}");
    }

    protected virtual void HandleDefeat()
    {

    }

    protected void SetPhase(int newPhase)
    {
        currentPhase = newPhase;

        Debug.Log($"{name}: Entered Phase {currentPhase}");

        OnPhaseChanged();
    }

}
