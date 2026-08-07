using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player1 : Player
{
    [Header("Life")]
    [SerializeField] public int playerHealth = 4;
    [SerializeField] private GameObject life1;
    [SerializeField] private GameObject life2;
    [SerializeField] private GameObject life3;
    [SerializeField] private GameObject life4;

    [Header("Other Fields")]
    [SerializeField] private UnityEvent lastLife;
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    protected override Vector2 ReadMovementInput() {

        float horizontal = 0f;
        float vertical = 0f;


        if (Input.GetKey("w"))
        {
            vertical++;
        }
        if (Input.GetKey("a"))
        {
            horizontal--;
        }
        if (Input.GetKey("s"))
        {
            vertical--;
        }
        if (Input.GetKey("d"))
        {
            horizontal++;
        }

        return new Vector2 (horizontal, vertical).normalized;
    }

    protected override void ReduceSharedHealth()
    {
        ReduceHealth();
    }

    // update health(battery). Called from player2 also.
    public void ReduceHealth()
    {
        playerHealth--;

        if(audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(this.gameObject.GetComponent<AudioSource>().clip);

        }

        switch(playerHealth)
        {
            case 3:
                life4.SetActive(false);
                break;

            case 2:
                life3.SetActive(false);
                break;

            case 1:
                life2.SetActive(false);
                break;

            case <= 0:
                life1.SetActive(false);
                playerManager.TriggerGameOver();
                break;
           
        }
    }

    protected override bool ReadShootInput()
    {
        // check if the player is holding the shooting key
        return Input.GetKey(KeyCode.Space);
    }

    
}
