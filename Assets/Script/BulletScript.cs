using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Vector3 thrust;
    // Start is called before the first frame update
    public AudioClip explosion;
    public bool isAlive = true;
    public virtual void Start()
    {
        this.GetComponent<Rigidbody>().AddRelativeForce(thrust);
    }
    private void Update()
    {
        if(gameObject.transform.position.z < -30f)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Die()
    {
        //Destroy(gameObject);
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        isAlive = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isAlive)
        {
            if (collision.collider.CompareTag("Boundary"))
            {
                Die();
            }
            if (collision.collider.CompareTag("AlienBullet"))
            {
                Die();
            }
            if (collision.collider.CompareTag("PlayerBullet"))
            {
                Die();
            }
            if (collision.collider.CompareTag("Shield"))
            {
                GameObject shield = collision.collider.gameObject;
                shield.GetComponent<ShieldScript>().TakeDamage();
                Die();
            }
            if (collision.collider.CompareTag("Player"))
            {
                collision.collider.gameObject.GetComponent<PlayerScript>().TakeDamage();
                AudioSource.PlayClipAtPoint(explosion, collision.collider.gameObject.transform.position);
                Die();
            }
        }

    }
}
