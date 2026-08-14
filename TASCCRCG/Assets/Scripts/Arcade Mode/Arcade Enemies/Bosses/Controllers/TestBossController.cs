using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossController : BossController
{
    [SerializeField] private Transform shootPoint;

    public override void StartFight()
    {
        base.StartFight();

        EnterIdleState();
    }

    public void EnterIdleState()
    {
        ChangeState(new TestIdleState(this));
    }

    public void EnterMovingState()
    {
        ChangeState(new TestMovingState(this));
    }

    public void ShootSaws()
    {
        Vector3 direction = Vector3.left;
        EnemyBullet bullet = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, 180f);
        bullet.Initialize(direction);

        direction = (Vector3.left + Vector3.up).normalized;
        EnemyBullet bullet2 = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
        bullet2.transform.position = shootPoint.position;
        bullet2.transform.rotation = Quaternion.Euler(0, 0, 135f);
        bullet2.Initialize(direction);

        direction = (Vector3.left + Vector3.down).normalized;
        EnemyBullet bullet3 = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
        bullet3.transform.position = shootPoint.position;
        bullet3.transform.rotation = Quaternion.Euler(0, 0, 225f);
        bullet3.Initialize(direction);


    }
}
