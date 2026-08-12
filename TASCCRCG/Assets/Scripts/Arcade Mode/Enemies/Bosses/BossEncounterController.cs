using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEncounterController : MonoBehaviour
{
    private BossController activeBoss;

    [Header("Timing")]
    [SerializeField] private float delayBeforeBoss = 2f;

    public bool IsEncounterRunning { get; private set; }
    public bool IsEncounterComplete { get; private set; }

    public event System.Action OnBossFightStarted;
    public event System.Action OnBossFightCompleted;

    public void StartEncounter(BossController bossPrefab)
    {
        if (IsEncounterRunning)
        {
            return;
        }

        if(bossPrefab == null)
        {
            Debug.LogError($"{name}: Cannot start encounter with a null boss.",this);
            return;
        }

        StartCoroutine(RunEncounter(bossPrefab));
    }

    private IEnumerator RunEncounter(BossController bossPrefab)
    {
        IsEncounterRunning = true;
        IsEncounterComplete = false;

        // TO DO: Implement scrolling stopage

        activeBoss = Instantiate(bossPrefab, new Vector3(2f, 0f, 0f), Quaternion.identity);

        OnBossFightStarted?.Invoke();

        activeBoss.StartFight();

        // Runs until boss defeat flag is up.
        while (!activeBoss.IsDefeated)
        {
            yield return null;
        }

        OnBossFightCompleted?.Invoke();

        // TO DO: Implement Boss Death or Exit

        IsEncounterRunning = false;
        IsEncounterComplete = true;

    }
}
