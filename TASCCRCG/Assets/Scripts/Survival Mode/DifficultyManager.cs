using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour // Used as global info for difficulty
{
    public int CurrentDifficulty { get; private set; }
    
    public void SetDifficulty(int difficulty)
    {
        CurrentDifficulty = difficulty;
    }
}
