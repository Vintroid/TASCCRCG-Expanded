using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public bool IsDefeated { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DefeatBoss();
        }
    }

    // Boss fight initialization
    public void StartFight()
    {
        IsDefeated = false;

        Instantiate(this,new Vector3(2f,0f,0f),Quaternion.Euler(0f,0f,0));

        Debug.Log("Boss fight started!");
    }

    // Boss beaten cleanup
    public void DefeatBoss()
    {
        IsDefeated = true;

        Debug.Log("Boss Defeated");
    }
}
