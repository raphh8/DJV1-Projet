using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingState : AbstractState
{
    public override void EnterState(PlayerCharacter manager)
    {
            manager.animator.SetTrigger("Shoot");
            manager.ShootBullet();
    }

    public override void UpdateState(PlayerCharacter manager)
    {
        if (manager.shooted)
        {
            manager.shooted = false;
            manager.SwitchState(manager.Walking);
        }
    }
}

