using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class WrenchWave : MonoBehaviour
{
    SurvivalGameManager gameManager;
    [SerializeField] GameObject wrench;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = this.GetComponent<SurvivalGameManager>();
    }

    public void Wave()
    {
        Debug.Log("Wrench Wave!");
        StartCoroutine(SpawnRoutine());
    }

    // Cooldown between wrenches during waves
    IEnumerator SpawnRoutine()
    {
        int wrenchNum = 2 + gameManager.difficulty;

        for (int i = 0; i < wrenchNum; i++)
        {
            // Random Y position on game space
            float xPos = 7.5f;
            float yPos = UnityEngine.Random.Range(1f, 6f) - 3.15f;

            GameObject.Instantiate(wrench, new Vector3(xPos, yPos, 0f), Quaternion.identity);
            yield return new WaitForSeconds(0.75f);
        }
    }
}
