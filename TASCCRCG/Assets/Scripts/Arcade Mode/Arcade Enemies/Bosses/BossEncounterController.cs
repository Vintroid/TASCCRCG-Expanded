using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEncounterController : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private ScrollingController scrollingController;

    [Header("Boss Entrance")]
    [SerializeField] private Vector3 bossSpawnPosition = new Vector3(10f, 0f, 0f);
    [SerializeField] private Vector3 bossFightPosition = new Vector3(3.5f, 0f, 0f);
    [SerializeField] private float bossEntranceSpeed = 3f;

    [Header("Player Intro")]
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    [SerializeField] private float playerBossIntroX = -3f;

    private BossController activeBoss;
    private bool bossDefeated;

    [Header("Timing")]
    [SerializeField] private float delayBeforeBoss = 2f;
    [SerializeField] private float delayBeforeBossAttack = 1f;


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

        // Screen stop scrolling
        if(scrollingController != null)
        {
            scrollingController.StopScrolling();
        }

        // Move players toward boss intro position
        if(player1 != null)
        {
            player1.StartBossIntroPositioning(playerBossIntroX);
        }

        if(player2 != null && player2.isActiveAndEnabled)
        {
            player2.StartBossIntroPositioning(playerBossIntroX);
        }

        // Pause before boss appearance
        yield return new WaitForSeconds(delayBeforeBoss);

        // Boss appears to designated coordinates
        activeBoss = Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);

        bossDefeated = false;
        activeBoss.OnBossDefeated += HandleBossDefeated; // Waiting for BossController event

        // Boss entrace. code stop until coroutine is over.
        yield return StartCoroutine(MoveBossIntoPosition());

        // Pause before boss attacks
        yield return new WaitForSeconds(delayBeforeBossAttack);

        // Players can move again
        if (player1 != null)
        {
            player1.EndBossIntroPositioning();
        }
        if (player2 != null && player2.isActiveAndEnabled)
        {
            player2.EndBossIntroPositioning();
        }

        // Start fight after entrance is over.
        OnBossFightStarted?.Invoke();

        activeBoss.StartFight();

        // Runs until boss defeat flag is up.
        while (!bossDefeated)
        {
            yield return null;
        }

        OnBossFightCompleted?.Invoke();

        IsEncounterRunning = false;
        IsEncounterComplete = true;

    }

    private IEnumerator MoveBossIntoPosition()
    {
        // Boss moving slowly towards starting position.
        while(Vector3.Distance(activeBoss.transform.position, bossFightPosition) > 0.01f)
        {
            activeBoss.transform.position =
                Vector3.MoveTowards(activeBoss.transform.position, bossFightPosition,
                    bossEntranceSpeed * Time.deltaTime);

            yield return null;
        }

        activeBoss.transform.position = bossFightPosition;
    }

    private void HandleBossDefeated(BossController boss)
    {
        bossDefeated = true;

        boss.OnBossDefeated -= HandleBossDefeated;
    }
}
