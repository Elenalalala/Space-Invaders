using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBulletScript : BulletScript
{
    public event Action OnBulletDestroyed;
    public override void Die()
    {
        base.Die();
        // TODO call back to parent player to enable bullet
        OnBulletDestroyed?.Invoke();
    }
}
