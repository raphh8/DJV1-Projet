using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingState : AbstractState
{
    public override void EnterState(MovementManager manager)
    {
        manager.animator.SetTrigger("Shoot");
        var direction = manager.transform.rotation * Vector3.forward;
        manager.ShootBullet(manager.transform.position + direction * 0.5f, manager.transform.rotation);
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

