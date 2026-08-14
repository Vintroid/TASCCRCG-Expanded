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

        direction = (Vector3.left + 2*Vector3.up).normalized;
        EnemyBullet bullet4 = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
        bullet4.transform.position = shootPoint.position;
        bullet4.transform.rotation = Quaternion.Euler(0, 0, 112.5f);
        bullet4.Initialize(direction);

        direction = (Vector3.left + 2*Vector3.down).normalized;
        EnemyBullet bullet5 = ProjectilePoolManager.Instance.GetProjectile(ProjectileType.Saw);
        bullet5.transform.position = shootPoint.position;
        bullet5.transform.rotation = Quaternion.Euler(0, 0, 247.5f);
        bullet5.Initialize(direction);


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
}
