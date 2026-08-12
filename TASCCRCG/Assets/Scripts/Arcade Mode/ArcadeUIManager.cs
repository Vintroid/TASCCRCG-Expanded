using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeUIManager : MonoBehaviour
{
    [SerializeField] private ArcadeLevelManager arcadeLevelManager;

    private void Update()
    {
        if (!arcadeLevelManager.IsLevelComplete)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ArcadeLevelDefinition nextLevel = arcadeLevelManager.CurrentLevel.NextLevel;

            if (nextLevel != null)
            {
                arcadeLevelManager.StartLevel(nextLevel);
            }
        }
    }

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

        ArcadeLevelDefinition nextLevel = arcadeLevelManager.CurrentLevel.NextLevel;

        if (nextLevel != null)
        {
            Debug.Log($"Next Level available: {nextLevel.name}. Press Enter to Continue.");
        }
        else
        {
            Debug.Log("No next level. Arcade Complete!");
        }
    }
}
