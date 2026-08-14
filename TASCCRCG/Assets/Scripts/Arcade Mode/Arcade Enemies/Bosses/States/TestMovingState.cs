using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovingState : BossState
{
    private TestBossController testBoss;

    private float timer;
    private const float centerY = 0f;
    private float moveDuration = 3f;
    private float moveSpeed = 1.5f;
    private float moveDistance = 2f;

    public TestMovingState(TestBossController boss) : base(boss)
    {
        testBoss = boss;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        testBoss.MovementTime += Time.deltaTime;

        float newY = centerY + Mathf.Sin(testBoss.MovementTime * moveSpeed) * moveDistance;

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
        moveDuration = testBoss.SetMovementDuration();

        timer = moveDuration;

        moveSpeed = testBoss.SetMovementSpeed();

        Debug.Log("Entered Moving State.");
    }

    public override void Exit()
    {
        Debug.Log("Exited Moving state");
    }
}
