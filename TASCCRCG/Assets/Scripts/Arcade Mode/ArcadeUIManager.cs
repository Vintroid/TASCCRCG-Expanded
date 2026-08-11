using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeUIManager : MonoBehaviour
{
    [SerializeField] private ArcadeLevelManager arcadeLevelManager;

    private void OnEnable()
    {
        if(arcadeLevelManager != null)
        {
            arcadeLevelManager.OnLevelCompleted += ShowLevelCompleteScreen;
        }
        
    }

    private void OnDisable()
    {
        if(arcadeLevelManager != null)
        {
            arcadeLevelManager.OnLevelCompleted -= ShowLevelCompleteScreen;
        }
    }

    private void ShowLevelCompleteScreen()
    {
        Debug.Log("Showing Completion Screen!");
    }
}
