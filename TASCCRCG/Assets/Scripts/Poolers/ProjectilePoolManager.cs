using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolManager : MonoBehaviour
{
    public static ProjectilePoolManager Instance { get; private set; }

    // EnemyBullet Pools instances
    [SerializeField] private EnemyBulletPool sawPool;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Functions to call different pools to create bullet type
    public EnemyBullet GetProjectile(ProjectileType type)
    {
        switch(type)
        {
            case ProjectileType.Saw:

                return sawPool.Get();

            default:
                Debug.LogError("Unknown projectile type.");
                return null;
        }
    }
}
