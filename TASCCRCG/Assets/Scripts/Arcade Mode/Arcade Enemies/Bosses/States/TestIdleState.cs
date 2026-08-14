using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIdleState : BossState
{
    private TestBossController testBoss;
    private float timer;
    private float shootTimer;
    private float idleDuration = 4f;
    private float shootInterval = 0.5f;

    public TestIdleState(TestBossController boss): base(boss) {

        testBoss = boss;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        shootTimer -= Time.deltaTime;

        if(shootTimer <= 0f)
        {
            testBoss.ShootSaws();
            shootTimer = shootInterval;
        }

        if(timer <= 0f)
        {
            testBoss.EnterMovingState();
        }
    }

    public override void Enter()
    {
        timer = idleDuration;
        shootTimer = 0f;
        shootInterval = testBoss.SetShootingInterval();

        Debug.Log("Entered Idle State");
    }

    public override void Exit()
    {
        Debug.Log("Exited Idle state");
    }

}
