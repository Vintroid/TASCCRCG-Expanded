using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEncounterController : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private BossController boss;

    [Header("Timing")]
    [SerializeField] private float delayBeforeBoss = 2f;

    public bool IsEncouterRunning { get; private set; }
    public bool IsEncouterComplete { get; private set; }

    public event System.Action OnBossFightStarted;
    public event System.Action OnBossFightCompleted;

    public void StartEncounter()
    {
        if (IsEncouterRunning)
        {
            return;
        }

        StartCoroutine(RunEncounter());
    }

    private IEnumerator RunEncounter()
    {
        IsEncouterRunning = true;
        IsEncouterComplete = false;

        // TO DO: Implement scrolling stopage

        OnBossFightStarted?.Invoke();

        boss.StartFight();

        // Runs until boss defeat flag is up.
        while (!boss.IsDefeated)
        {
            yield return null;
        }

        OnBossFightCompleted?.Invoke();

        // TO DO: Implement Boss Death or Exit

        IsEncouterRunning = false;
        IsEncouterComplete = true;

    }
}
