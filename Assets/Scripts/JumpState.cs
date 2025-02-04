using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AbstractState
{
    public override void EnterState(PlayerCharacter manager)
    {
        manager.animator.SetTrigger("Jump");
    }

    public override void UpdateState(PlayerCharacter manager)
    {
        if(manager.jumped && manager.IsOnGround())
        {
            manager.jumped = false;
            if (manager.horizontal == 0 && manager.vertical == 0)
            {
                manager.SwitchState(manager.Idle);
            }
            else if (Input.GetKey(KeyCode.LeftShift) && manager.bonus1) manager.SwitchState(manager.Running);
            else manager.SwitchState(manager.Walking);
        }
    }
}
