using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool : MonoBehaviour
{
    // Each pooler instance take care of 1 prefab
    [SerializeField] EnemyBullet prefab;
    [SerializeField] int defaultCapacity = 50;
    [SerializeField] int maxCapacity = 100;

    private IObjectPool<EnemyBullet> pool;

    private void Awake()
    {
        // Initializing functions related to pool instance
        pool = new ObjectPool<EnemyBullet>(

            createFunc: CreateBullet,
            actionOnGet: OnGetBullet,
            actionOnRelease: OnReleaseBullet,
            actionOnDestroy: OnDestroyBullet,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxCapacity

            );
    }

    public EnemyBullet Get()
    {
        return pool.Get();
    }

    public void Release(EnemyBullet bullet)
    {
        pool.Release(bullet);
    }

    private EnemyBullet CreateBullet()
    {
        EnemyBullet bullet = Instantiate(prefab, transform);

        // We want the bullet to have access to the pool.
        bullet.SetPool(pool);

        return bullet;
    }

    private void OnGetBullet(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    
    private void OnReleaseBullet(EnemyBullet bullet) {

        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }


}
