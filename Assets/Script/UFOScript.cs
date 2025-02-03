using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOScript : AlienScript
{
    public Material goldenMaterial;
    private void Start()
    {
        global = GameObject.Find("Global").GetComponent<Gobal>();
        Move(Vector3.right * 200f);
    }
    public override void Move(Vector3 dir)
    {
        this.GetComponent<Rigidbody>().AddRelativeForce(dir);
        
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PlayerBullet"))
        {

            BulletScript bullet = collision.collider.gameObject.GetComponent<BulletScript>();
            if (bullet.isAlive && isAlive)
            {
                bullet.Die();


                //if (ReallyDie()) return;
                Die();
                global.AddScore(points);
                global.AddSkill();
            }

        }
        if (collision.collider.CompareTag("Boundary"))
        {
            this.gameObject.GetComponent<Renderer>().material = goldenMaterial;
        }
        if (collision.collider.CompareTag("Player"))
        {
            if(this.gameObject.GetComponent<Renderer>().material == goldenMaterial)
            {
                // get absorbed by the player
                collision.collider.gameObject.GetComponent<PlayerScript>().AbsorbBulletRainSkill();
                Destroy(gameObject);

            }
        }
    }


}
