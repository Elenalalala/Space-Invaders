using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOScript : AlienScript
{
    public AudioClip UFOFlying;
    public AudioClip UFOTurnSkill;
    private void Start()
    {
        global = GameObject.Find("Global").GetComponent<Gobal>();
        Move(Vector3.right * 300f);
    }
    public override void Move(Vector3 dir)
    {
        this.GetComponent<Rigidbody>().AddRelativeForce(dir);
        audioSource.clip = UFOFlying;
        audioSource.Play();

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
            }

        }
        if (collision.collider.CompareTag("Boundary"))
        {
            if(collision.collider.gameObject.name == "RightBound")
            {
                Destroy(this.gameObject);
            }
            this.gameObject.GetComponent<Renderer>().material = alienGold;
            audioSource.clip = UFOTurnSkill;
            audioSource.Play();
        }
        if (collision.collider.CompareTag("Player"))
        {
            if (this.gameObject.GetComponent<Renderer>().sharedMaterial == alienGold)
            {
                // get absorbed by the player
                collision.collider.gameObject.GetComponent<PlayerScript>().AbsorbLife();
                Destroy(gameObject);

            }
        }
    }


}
