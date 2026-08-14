using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossController : BossController
{
    [Header("Test Boss Music")]
    [SerializeField] private AudioClip bossMusic;
    private AudioSource musicSource;
    private AudioClip previousMusic;
    private float previousMusicTime;

    // 3 Phases to account for
    [Header("Phase Behaviour")]
    [SerializeField] private int phase1Threshold = 20;
    [SerializeField] private int phase2Threshold = 10;

    [SerializeField] private Transform shootPoint;

    public override void StartFight()
    {
        base.StartFight();


        if(Camera.main != null)
        {
            // Set up boss music
            musicSource = Camera.main.GetComponent<AudioSource>();
        }

        if(musicSource != null && bossMusic != null)
        {
            previousMusic = musicSource.clip;
            previousMusicTime = musicSource.time;

            musicSource.Stop();
            musicSource.clip = bossMusic;
            musicSource.time = 0f;
            musicSource.Play();
        }

        EnterIdleState();
    }

    public void EnterIdleState()
    {
        ChangeState(new TestIdleState(this));
    }

    public void EnterMovingState()
    {
        ChangeState(new TestMovingState(this));
    }

    public void ShootSaws()
    {
        Vector3 direction = Vector3.left;
        EnemyBullet bullet = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, 180f);
        bullet.Initialize(direction);

        direction = (Vector3.left + Vector3.up).normalized;
        EnemyBullet bullet2 = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
        bullet2.transform.position = shootPoint.position;
        bullet2.transform.rotation = Quaternion.Euler(0, 0, 135f);
        bullet2.Initialize(direction);

        direction = (Vector3.left + Vector3.down).normalized;
        EnemyBullet bullet3 = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
        bullet3.transform.position = shootPoint.position;
        bullet3.transform.rotation = Quaternion.Euler(0, 0, 225f);
        bullet3.Initialize(direction);

        if(CurrentPhase == 2)
        {
            direction = (Vector3.up).normalized;
            EnemyBullet bullet4 = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
            bullet4.transform.position = shootPoint.position;
            bullet4.transform.rotation = Quaternion.Euler(0, 0, 90f);
            bullet4.Initialize(direction);

            direction = (Vector3.down).normalized;
            EnemyBullet bullet5 = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
            bullet5.transform.position = shootPoint.position;
            bullet5.transform.rotation = Quaternion.Euler(0, 0, 270f);
            bullet5.Initialize(direction);
        }

        if (CurrentPhase == 3) {

            direction = (Vector3.left + 2 * Vector3.up).normalized;
            EnemyBullet bullet6 = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
            bullet6.transform.position = shootPoint.position;
            bullet6.transform.rotation = Quaternion.Euler(0, 0, 112.5f);
            bullet6.Initialize(direction);

            direction = (Vector3.left + 2 * Vector3.down).normalized;
            EnemyBullet bullet7 = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
            bullet7.transform.position = shootPoint.position;
            bullet7.transform.rotation = Quaternion.Euler(0, 0, 247.5f);
            bullet7.Initialize(direction);

            direction = (Vector3.right + 2 * Vector3.up).normalized;
            EnemyBullet bullet8 = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
            bullet8.transform.position = shootPoint.position;
            bullet8.transform.rotation = Quaternion.Euler(0, 0, 67.5f);
            bullet8.Initialize(direction);

            direction = (Vector3.right + 2 * Vector3.down).normalized;
            EnemyBullet bullet9 = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
            bullet9.transform.position = shootPoint.position;
            bullet9.transform.rotation = Quaternion.Euler(0, 0, 292.5f);
            bullet9.Initialize(direction);

        }


    }

    protected override void HandleDefeat()
    {
        if(musicSource != null && previousMusic != null)
        {
            musicSource.Stop();

            musicSource.clip = previousMusic;
            musicSource.time = previousMusicTime;

            musicSource.Play();
        }

        base.HandleDefeat();
        Destroy(gameObject);
    }

    // Handling
    protected override void OnHealthChanged()
    {
        base.OnHealthChanged();

        if(currentPhase == 1 && CurrentHealth <= phase1Threshold)
        {
            SetPhase(2);
        }

        else if(currentPhase == 2 && CurrentHealth <= phase2Threshold)
        {
            SetPhase(3);
        }
    }

    // Phases change shooting speed
    public float SetShootingInterval()
    {
        switch (CurrentPhase)
        {
            case 1:
                return 1f;

            case 2:
                return 0.7f;

            case 3:
                return 0.4f;

            default:
                return 1f;
        }
    }

    // Phases change movement speed
    public float SetMovementSpeed()
    {
        switch (CurrentPhase)
        {
            case 1:
                return 2f;

            case 2:
                return 3f;

            case 3:
                return 4f;

            default:
                return 2f;
               
        }
    }

    // Phases change movement duration
    public float SetMovementDuration()
    {
        switch (CurrentPhase)
        {
            case 1:
                return 3f;

            case 2:
                return 2.5f;

            case 3:
                return 2f;

            default:
                return 3f;
        }
        
    }
}
