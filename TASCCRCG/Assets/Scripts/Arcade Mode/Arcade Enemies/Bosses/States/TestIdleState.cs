using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIdleState : BossState
{
    private TestBossController testBoss;
    private float timer;
    private const float idleDuration = 2f;
    public TestIdleState(TestBossController boss): base(boss) {

        testBoss = boss;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0f)
        {
            testBoss.EnterMovingState();
        }
    }

    public override void Enter()
    {
        timer = idleDuration;

        Debug.Log("Entered Idle State");
    }

    public override void Exit()
    {
        Debug.Log("Exited Idle state");
    }
}
