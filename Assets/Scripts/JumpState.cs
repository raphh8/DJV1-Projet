using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AbstractState
{
    public override void EnterState(MovementManager manager)
    {
        manager.animator.SetTrigger("Jump");
    }

    public override void UpdateState(MovementManager manager)
    {
        if(manager.jumped && manager.IsOnGround())
        {
            manager.jumped = false;
            if (manager.horizontal == 0 && manager.vertical == 0)
            {
                manager.SwitchState(manager.Idle);
            }
            else if (Input.GetKey(KeyCode.LeftShift)) manager.SwitchState(manager.Running);
            else manager.SwitchState(manager.Walking);
        }
    }
}
