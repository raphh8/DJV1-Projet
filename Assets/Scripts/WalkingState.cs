using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : AbstractState
{
    public override void EnterState(PlayerCharacter manager)
    {
        manager.animator.SetBool("Walking", true);
    }


    public override void UpdateState(PlayerCharacter manager)
    {
        if (Input.GetKey(KeyCode.LeftShift)) ExitState(manager, manager.Running);
        else if (manager.dir.magnitude < 0.1f) ExitState(manager, manager.Idle);

        if (Input.GetKey(KeyCode.Space)) ExitState(manager, manager.Jump);

        if (Input.GetMouseButtonDown(0)) ExitState(manager, manager.Shoot);
    }

    void ExitState(PlayerCharacter manager, AbstractState state)
    {
        manager.animator.SetBool("Walking", false);
        manager.SwitchState(state);
    }
}
