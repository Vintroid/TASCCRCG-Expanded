using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPawnBulletSpawner : PawnBulletSpawner
{
    protected override void Shoot()
    {
        // down pawn
        SpawnBullet((Vector3.right + Vector3.down).normalized, 270f);

    }
}
