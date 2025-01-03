using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : AbstractState
{
    public override void EnterState(MovementManager manager)
    {
        manager.animator.SetBool("Running", true);
    }


    public override void UpdateState(MovementManager manager)
    {
        if(Input.GetKeyUp(KeyCode.LeftShift)) ExitState(manager, manager.Walking);
        else if (manager.dir.magnitude < 0.1f) ExitState(manager, manager.Idle);
    }
    void ExitState(MovementManager manager, AbstractState state)
    {
        manager.animator.SetBool("Running", false);
        manager.SwitchState(state);
    }
}

