using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossController : BossController
{
    public override void StartFight()
    {
        base.StartFight();

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
}
