using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGear : Enemy
{

    // Enemy unique fields
    [Header("Characteristics")]
    [SerializeField] int baseHealth = 8;
    [SerializeField] int baseScoreValue = 200;
    [SerializeField] float speed = 1f;

    private float shootingTime = 0f;
    private bool isShooting;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = baseHealth + difficultyManager.CurrentDifficultyTier;
        scoreValue = baseScoreValue;
        isShooting = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        shootingTime += Time.deltaTime;

        // Moving in increments
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Shooting
        if (!isShooting & shootingTime >= 5f)
        {
            StartCoroutine(Shoot());
            shootingTime = 0f;
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        for(int i=0; i<3; i++)
        { 
            // Radial pattern 0 to 180.
            SpawnProjectile(ProjectileType.Saw, Vector3.right, 0);
            SpawnProjectile(ProjectileType.Saw, Vector3.up + Vector3.right, 60);
            SpawnProjectile(ProjectileType.Saw, Vector3.up, 105);
            SpawnProjectile(ProjectileType.Saw, Vector3.up + Vector3.left, 160);
            SpawnProjectile(ProjectileType.Saw, Vector3.left, 180);

            scoreManager.AddScore(75);
            yield return new WaitForSeconds(0.33f);
        }

        isShooting = false;
        

    }
    
}
