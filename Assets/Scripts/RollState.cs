using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : AbstractState
{
    public override void EnterState(PlayerCharacter manager)
    {
        manager.animator.SetTrigger("Roll");
        manager.RollPerform();
    }

    public override void UpdateState(PlayerCharacter manager)
    {
        if (manager.rolled && manager.IsOnGround())
        {
            manager.rolled = false;
            manager.SwitchState(manager.Walking);
        }
    }
}
