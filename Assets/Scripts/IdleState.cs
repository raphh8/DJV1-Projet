using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AbstractState
{
    public override void EnterState(MovementManager manager)
    {

    }


    public override void UpdateState(MovementManager manager)
    {
        if (manager.dir.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift)) manager.SwitchState(manager.Running);
            manager.SwitchState(manager.Walking);
        }
        if(Input.GetKey(KeyCode.Space)) manager.SwitchState(manager.Jump);

        if (Input.GetMouseButtonDown(0)) manager.SwitchState(manager.Shoot);
    }
}
