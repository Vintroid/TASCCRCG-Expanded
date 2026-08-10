using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour // Use as shared access to playerManager scoring
{
    [SerializeField] private PlayerManager playerManager;

    public void AddScore(int amount)
    {
        playerManager.AddScore(amount);
    }
}
