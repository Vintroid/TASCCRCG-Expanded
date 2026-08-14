using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovingState : BossState
{
    private TestBossController testBoss;

    private float timer;
    private float startY;

    private const float moveDuration = 3f;
    private const float moveSpeed = 2f;
    private const float moveDistance = 1.5f;

    public TestMovingState(TestBossController boss) : base(boss)
    {
        testBoss = boss;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;

        float newY = startY + Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        boss.transform.position = new Vector3(
            boss.transform.position.x,
            newY,
            boss.transform.position.z
        );

        if(timer <= 0f)
        {
            testBoss.EnterIdleState();
        }
    }

    public override void Enter()
    {
        timer = moveDuration;
        startY = boss.transform.position.y;

        Debug.Log("Entered Moving State.");
    }

    public override void Exit()
    {
        Debug.Log("Exited Moving state");
    }
}
