using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGear : Enemy
{

    // Enemy unique fields
    [Header("Characteristics")]
    [SerializeField] int baseHealth = 5;
    [SerializeField] int baseScoreValue = 200;
    [SerializeField] float speed = 1f;

    private float shootingTime = 0f;
    private bool isShooting;

    // Prefabs
    [SerializeField] GameObject saw;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = baseHealth + gameManager.difficulty;
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
            SpawnProjectile(saw,Vector3.left, 0);
            SpawnProjectile(saw,Vector3.up + Vector3.left, 45);
            SpawnProjectile(saw,Vector3.up, 90);
            SpawnProjectile(saw, Vector3.up + Vector3.right, 135);
            SpawnProjectile(saw, Vector3.right, 180);

            gameManager.playerManager.AddScore(75);
            yield return new WaitForSeconds(0.33f);
        }

        isShooting = false;
        

    }
    
}
