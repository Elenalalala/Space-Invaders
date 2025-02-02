using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Vector3 thrust;
    // Start is called before the first frame update
    public AudioClip explosion;
    public virtual void Start()
    {
        this.GetComponent<Rigidbody>().AddRelativeForce(thrust);
    }

    public virtual void Die()
    {
        //Destroy(gameObject);
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Boundary") || collision.collider.CompareTag("AlienBullet") || collision.collider.CompareTag("PlayerBullet"))
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
