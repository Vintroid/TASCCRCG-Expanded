using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Enemy
{
    Animator animator;

    // Enemy unique fields
    [Header("Characteristics")]
    [SerializeField] int baseHealth = 3;
    [SerializeField] int baseScoreValue = 100;
    [SerializeField] float minMoveAmp = 0.5f; // Amp Tested Default 0.5f to 3f.
    [SerializeField] float maxMoveAmp = 3f;
    [SerializeField] float fireRate = 2.5f;

    private float time = 0f;
    private float amp;
    private float verticalDirection;
    private float nextShotTime = 0f;
    private bool isShooting;

    protected override void Awake()
    {
        base.Awake();
        animator = this.GetComponent<Animator>();
        isShooting = false;
        nextShotTime = time + fireRate;

        // Allow starting upwards or downwards
        verticalDirection = UnityEngine.Random.value < 0.5f ? -1f : 1f;
    }

    // Start is called before the first frame update.
    protected override void Start()
    {
        base.Start();
        health = baseHealth + difficultyManager.CurrentDifficulty;
        scoreValue = baseScoreValue;

        // Random amplitude
        amp = UnityEngine.Random.Range(minMoveAmp, maxMoveAmp);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // Setting animator speed
        if (!isShooting)
        {

            time += Time.deltaTime;

            // Sin movement
            float x = 2 * time;
            float y = (float)(amp * (Math.Sin(x))) * verticalDirection;
            animator.SetFloat("Vert_speed", y);

            // Moving in increments
            transform.position = new Vector3(7.5f, 0f, 0f) - new Vector3(x, y, 0f);

        }

        // Shooting check
        if (!isShooting && time >= nextShotTime)
        {
            StartCoroutine(Shoot());
            nextShotTime = time + fireRate;
        }

    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        animator.SetFloat("Vert_speed", 0f);
        animator.SetTrigger("shoot");
        yield return new WaitForSeconds(1.5f);

        // Shooting the saw octagonally
        DetachSaws();

        yield return new WaitForSeconds(0.75f);

        animator.ResetTrigger("shoot");

        isShooting = false;

    }

    private void DetachSaws()
    {
        // Octagonal pattern
        SpawnProjectile(ProjectileType.Saw, Vector3.right, 0);
        SpawnProjectile(ProjectileType.Saw, Vector3.up + Vector3.right, 45);
        SpawnProjectile(ProjectileType.Saw, Vector3.up, 90);
        SpawnProjectile(ProjectileType.Saw, Vector3.up + Vector3.left, 135);
        SpawnProjectile(ProjectileType.Saw, Vector3.left, 180);
        SpawnProjectile(ProjectileType.Saw, Vector3.down + Vector3.left, 225);
        SpawnProjectile(ProjectileType.Saw, Vector3.down, 270);
        SpawnProjectile(ProjectileType.Saw, Vector3.down + Vector3.right, 315);

        scoreManager.AddScore(40);
    }
}
