using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour // Used as global info for difficulty
{
    public int CurrentDifficultyTier { get; private set; }
    public float WaveInterval { get; private set; }

    // Difficulty will work in tiers (0 through 7)
    public void SetDifficultyTier(int tier)
    {
        CurrentDifficultyTier = tier;

        switch (tier)
        {
            case 0:
                WaveInterval = 9f;
                break;

            case 1:
                WaveInterval = 8f;
                break;

            case 2:
                WaveInterval = 7f;
                break;

            case 3:
                WaveInterval = 6f;
                break;

            case 4:
                WaveInterval = 5f;
                break;

            case 5:
                WaveInterval = 4.5f;
                break;

            case 6:
                WaveInterval = 4f;
                break;

            case 7:
                WaveInterval = 3.5f;
                break;

            default:
                WaveInterval = 3f;
                break;
        }
    }
}
