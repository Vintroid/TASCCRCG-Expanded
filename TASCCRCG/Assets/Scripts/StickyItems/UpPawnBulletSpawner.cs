using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpPawnBulletSpawner : PawnBulletSpawner
{
    protected override void Shoot()
    {
        // up pawn
        SpawnBullet((Vector3.right + Vector3.up).normalized, 0f);

    }
}
