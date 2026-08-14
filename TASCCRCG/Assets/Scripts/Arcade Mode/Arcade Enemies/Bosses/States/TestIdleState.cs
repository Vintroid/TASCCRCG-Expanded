using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIdleState : BossState
{
    public TestIdleState(BossController boss): base(boss) { }

    public override void Update()
    {
        // Idle behaviour
    }

    public override void Enter()
    {
        Debug.Log("Entered Idle State");
    }

    public override void Exit()
    {
        Debug.Log("Exited Idle state");
    }
}
