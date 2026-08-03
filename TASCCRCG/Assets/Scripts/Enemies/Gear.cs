using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Enemy
{
    Animator animator;

    // Enemy unique fields
    [SerializeField] int baseHealth = 3;
    [SerializeField] int baseScoreValue = 100;
    [SerializeField] float minMoveAmp = 0.5f; // Amp Tested Default 0.5f to 3f.
    [SerializeField] float maxMoveAmp = 3f;

    private float time = 0f;
    private float amp;
    private float shootingTime = 0f;
    private float chargeTime = 0f;
    private bool shoot;

    // Prefabs
    [SerializeField] GameObject saw;

    protected override void Awake()
    {
        base.Awake();
        animator = this.GetComponent<Animator>();
        shoot = false;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = baseHealth + gameManager.difficulty;
        scoreValue = baseScoreValue;

        // Random amplitude
        amp = UnityEngine.Random.Range(minMoveAmp, maxMoveAmp);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // Setting animator speed
        if (!shoot) {

            time += Time.deltaTime;
            shootingTime += Time.deltaTime;

            // Cancelling charge time to timer
            time -= chargeTime;
            chargeTime = 0f;

            // Sin movement
            float x = 2 * time;
            float y = (float)(amp * (Math.Sin(x)));
            animator.SetFloat("Vert_speed", y);

            // Moving in increments
            transform.position = new Vector3(7.5f, 0f, 0f) - new Vector3(x, y, 0f);
        }

        // Shooting
        if (shootingTime >= 2f)
        {
            StartCoroutine(Shoot());
            shootingTime = 0f;
        }
    }

    private IEnumerator Shoot()
    {
        shoot = true;
        animator.SetFloat("Vert_speed", 0f);
        animator.SetTrigger("shoot");
        yield return new WaitForSeconds(2f);

        // Shooting the saw octagonally
        DetachSaws();

        yield return new WaitForSeconds(0.75f);

        animator.ResetTrigger("shoot");
        chargeTime += Time.deltaTime;

        shoot = false;

    }

    private void DetachSaws()
    {
        GameObject bulletNW = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, 135));
        bulletNW.GetComponent<EnemyBullet>().direction = new Vector3(-1, 1, 0).normalized;

        GameObject bulletNE = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, 45));
        bulletNE.GetComponent<EnemyBullet>().direction = new Vector3(1, 1, 0).normalized;

        GameObject bulletSE = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, -45));
        bulletSE.GetComponent<EnemyBullet>().direction = new Vector3(1, -1, 0).normalized;

        GameObject bulletSW = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, -135));
        bulletSW.GetComponent<EnemyBullet>().direction = new Vector3(-1, -1, 0).normalized;

        GameObject bulletN = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, 90));
        bulletN.GetComponent<EnemyBullet>().direction = new Vector3(0, 1, 0).normalized;

        GameObject bulletE = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, 0));
        bulletE.GetComponent<EnemyBullet>().direction = new Vector3(1, 0, 0).normalized;

        GameObject bulletS = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, -90));
        bulletS.GetComponent<EnemyBullet>().direction = new Vector3(0, -1, 0).normalized;

        GameObject bulletW = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, 180));
        bulletW.GetComponent<EnemyBullet>().direction = new Vector3(-1, 0, 0).normalized;

        gameManager.playerManager.AddScore(40);
    }
}
