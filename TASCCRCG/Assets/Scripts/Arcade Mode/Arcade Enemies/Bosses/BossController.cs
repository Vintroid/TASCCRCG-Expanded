using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 30;
    
    public int CurrentHealth { get; private set; }
    public bool IsDefeated { get; private set; }

    private void Update()
    {
        // Test instakill
        if (Input.GetKeyDown(KeyCode.O))
        {
            DefeatBoss();
        }
    }

    // Boss fight initialization
    public void StartFight()
    {
        CurrentHealth = maxHealth;
        IsDefeated = false;

        Debug.Log("Boss fight started! HP:{CurrentHealth}");
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
}
