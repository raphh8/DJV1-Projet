using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : AbstractState
{
    public override void EnterState(MovementManager manager)
    {
        manager.animator.SetBool("Walking", true);
    }


    public override void UpdateState(MovementManager manager)
    {
        if (Input.GetKey(KeyCode.LeftShift)) ExitState(manager, manager.Running);
        else if (manager.dir.magnitude < 0.1f) ExitState(manager, manager.Idle);
    }

    void ExitState(MovementManager manager, AbstractState state)
    {
        manager.animator.SetBool("Walking", false);
        manager.SwitchState(state);
    }
}
