using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : BulletScript
{
    private PlayerScript player;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    public override void Die()
    {
        base.Die();
        // TODO call back to parent player to enable bullet
        player.hasActiveBullet = true;
    }
}
