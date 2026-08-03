using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wrench : Enemy
{
    // Enemy unique fields
    [SerializeField] int baseHealth = 2;
    [SerializeField] int baseScoreValue = 50;
    [SerializeField] float speed = 2f;

    protected override void Start()
    {
        base.Start();
        health = baseHealth + gameManager.difficulty;
        scoreValue = baseScoreValue;
    }

    protected override void Update()
    {
        base.Update();
        // Enemy movement
        transform.position += Vector3.left * Time.deltaTime * speed;
    }
}
