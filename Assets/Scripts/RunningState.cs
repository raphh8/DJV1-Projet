using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : AbstractState
{
    public override void EnterState(PlayerCharacter manager)
    {
        manager.animator.SetBool("Running", true);
    }


    public override void UpdateState(PlayerCharacter manager)
    {
        if(Input.GetKeyUp(KeyCode.LeftShift)) ExitState(manager, manager.Walking);
        else if (manager.dir.magnitude < 0.1f) ExitState(manager, manager.Idle);

        if (Input.GetKey(KeyCode.Space)) ExitState(manager, manager.Jump);

        if (Input.GetMouseButtonDown(0)) ExitState(manager, manager.Shoot);

        if (Input.GetKey(KeyCode.LeftControl) && manager.bonus2) manager.SwitchState(manager.Roll);
    }
    void ExitState(PlayerCharacter manager, AbstractState state)
    {
        manager.animator.SetBool("Running", false);
        manager.SwitchState(state);
    }
}

