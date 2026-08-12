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

        Debug.Log("Boss fight started!");
    }

    // Boss beaten cleanup
    public void DefeatBoss()
    {
        IsDefeated = true;

        Debug.Log("Boss Defeated");
    }
}
