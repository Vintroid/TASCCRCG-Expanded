using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBulletPool : MonoBehaviour
{
    [Header("Bullet Prefabs")]
    [SerializeField] private PlayerBullet straightBulletPrefab;
    [SerializeField] private PlayerBullet diagonalBulletPrefab;

    [Header("Initial Pool Sizes")]
    [SerializeField] private int straightPoolSize = 20;
    [SerializeField] private int diagonalPoolSize = 20;

    // Using queues to store bullet instances
    private readonly Queue<PlayerBullet> straightPool = new();
    private readonly Queue<PlayerBullet> diagonalPool = new();

    private void Awake()
    {
        CreatePool(straightBulletPrefab, straightPool, straightPoolSize);
        CreatePool(diagonalBulletPrefab, diagonalPool, diagonalPoolSize);
    }
   
    private void CreatePool(PlayerBullet prefab, Queue<PlayerBullet> pool, int amount)
    {
        if(prefab == null)
        {
            Debug.LogError($"{name}: Bullet prefab is missing.",this);
            return;
        }

        // Filling pool with inactive playerBullet up to initial capacity
        for(int i=0; i<amount; i++)
        {
            PlayerBullet playerBullet = CreateBullet(prefab);
            playerBullet.gameObject.SetActive(false);
            pool.Enqueue(playerBullet);
        }
    }

    private PlayerBullet CreateBullet(PlayerBullet prefab)
    {
        PlayerBullet bullet = Instantiate(prefab, transform);

        return bullet;
    }

    public PlayerBullet GetStraightBullet()
    {
        return GetBullet(straightPool, straightBulletPrefab);
    }

    public PlayerBullet GetDiagonalBullet()
    {
        return GetBullet(diagonalPool, diagonalBulletPrefab);
    }

    // Retrieving from specific queues a bullet
    private PlayerBullet GetBullet(Queue<PlayerBullet> pool, PlayerBullet prefab)
    {
        PlayerBullet bullet;

        if(pool == null)
        {
            Debug.LogError($"{pool} is null. Cannot retrive bullet.");
            return null;
        }

        if(pool.Count > 0)
        {
            bullet = pool.Dequeue();
        }
        else
        {
            bullet = CreateBullet(prefab); // Empty pool, need to Instantiate
        }

        bullet.gameObject.SetActive(false);
        return bullet;
    }

    // Returning bullet to specific pool type depending on bullet type
    public void ReturnBullet(PlayerBullet bullet)
    {
        if(bullet == null)
        {
            return;
        }

        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform);

        switch(bullet.BulletType)
        {
            case PlayerBulletType.Straight:
                straightPool.Enqueue(bullet);
                break;

            case PlayerBulletType.Diagonal:
                diagonalPool.Enqueue(bullet);
                break;
        }
    }

    // Spawning bullet by getting them from pools
    public PlayerBullet SpawnBullet(PlayerBulletType bulletType, Vector3 position, Vector3 direction, float angle)
    {
        PlayerBullet bullet;

        switch (bulletType)
        {
            case PlayerBulletType.Straight:
                bullet = GetBullet(straightPool,straightBulletPrefab);
                break;

            case PlayerBulletType.Diagonal:
                bullet = GetBullet(diagonalPool,diagonalBulletPrefab);
                break;

            default: 
                return null;

        }

        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.Euler(0,0,angle);
        bullet.Initialize(direction, this, bulletType);

        return bullet;
    }
}
