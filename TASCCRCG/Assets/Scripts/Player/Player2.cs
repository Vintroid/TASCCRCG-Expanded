using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : Player
{
    [Header("Fields from Player1")]
    [SerializeField] private Player1 player1;

    protected override void Awake()
    {
        base.Awake();
        
        // Checking if player1 exists
        if(player1 == null)
        {
            Debug.LogError($"{name}: Player1 reference has not been assigned.");
        }
    }

    protected override Vector2 ReadMovementInput()
    {

        float horizontal = 0f;
        float vertical = 0f;


        if (Input.GetKey(KeyCode.UpArrow))
        {
            vertical++;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("Player 2 left");

            horizontal--;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            vertical--;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontal++;
        }

        return new Vector2(horizontal, vertical).normalized;
    }

    protected override bool ReadShootInput()
    {
        // check if the player is holding the shooting key
        return Input.GetKey(KeyCode.Period);
    }

    // Player2 sends call to Player1 when damaged
    protected override void ReduceSharedHealth()
    {
        player1.ReduceHealth();
    }
    
    // For last life synchronization
    public void BlinkRedWarning()
    {
        StartCoroutine(BlinkRedRoutine());
    }

}
