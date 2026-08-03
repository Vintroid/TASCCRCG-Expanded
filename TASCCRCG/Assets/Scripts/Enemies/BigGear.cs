using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGear : Enemy
{

    // Enemy unique fields
    [SerializeField] int baseHealth = 5;
    [SerializeField] int baseScoreValue = 200;
    [SerializeField] float speed = 1f;

    private float shootingTime = 0f; 

    // Prefabs
    [SerializeField] GameObject saw;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = baseHealth + gameManager.difficulty;
        scoreValue = baseScoreValue;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        shootingTime += Time.deltaTime;

        // Moving in increments
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Shooting
        if (shootingTime >= 5f)
        {
            StartCoroutine(Shoot());
            shootingTime = 0f;
        }
    }

    IEnumerator Shoot()
    {
        for(int i=0; i<3; i++)
        {
            GameObject bulletNW = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, 135));
            bulletNW.GetComponent<EnemyBullet>().direction = new Vector3(-1, 1, 0).normalized;

            GameObject bulletNE = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, 45));
            bulletNE.GetComponent<EnemyBullet>().direction = new Vector3(1, 1, 0).normalized;

            GameObject bulletN = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, 90));
            bulletN.GetComponent<EnemyBullet>().direction = new Vector3(0, 1, 0).normalized;

            GameObject bulletE = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, 0));
            bulletE.GetComponent<EnemyBullet>().direction = new Vector3(1, 0, 0).normalized;

            GameObject bulletW = Instantiate(saw, transform.position, Quaternion.Euler(0, 0, 180));
            bulletW.GetComponent<EnemyBullet>().direction = new Vector3(-1, 0, 0).normalized;

            gameManager.playerManager.AddScore(75);
            yield return new WaitForSeconds(0.33f);
        }
        

    }
    
}
