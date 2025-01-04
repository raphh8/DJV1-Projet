using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingState : AbstractState
{
    public override void EnterState(MovementManager manager)
    {
        manager.animator.SetTrigger("Shoot");
    }

    public override void UpdateState(MovementManager manager)
    {
        if (manager.shooted)
        {
            manager.shooted = false;
            manager.SwitchState(manager.Walking);
        }
    }
}

