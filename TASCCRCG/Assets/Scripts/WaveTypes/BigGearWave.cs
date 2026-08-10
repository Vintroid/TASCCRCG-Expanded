using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGearWave : MonoBehaviour
{
    SurvivalGameManager gameManager;
    [SerializeField] GameObject bigGear;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = this.GetComponent<SurvivalGameManager>();
    }

    public void Wave()
    {
        Debug.Log("BigGear Wave!");
        // Position on ground
        float xPos = 8f;
        float yPos = -3.33f;

        GameObject.Instantiate(bigGear, new Vector3(xPos, yPos, 0f), Quaternion.identity);
    }
}
