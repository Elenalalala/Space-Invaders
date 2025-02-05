using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int activeBullet = 1;
    public GameObject playerBullet;
    public float speed;
    public GameObject deathExplosion;

    public AudioClip deathSound;
    public AudioClip shootSound;

    private AudioSource audioSource;

    private Gobal global;
    // Start is called before the first frame update
    void Start()
    {
        global = GameObject.Find("Global").GetComponent<Gobal>();
        StartCoroutine(BlinkAndDie(0.8f, 0.2f));
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.z < -30f)
        {
            TakeDamage();
            Instantiate(this.gameObject, new Vector3(0, 0, -10.5f), Quaternion.identity);
            Destroy(this.gameObject);
        }
            if (Input.GetKeyDown(KeyCode.Q))
        {
            global.lives += 10;
            activeBullet += 100;
            global.UpdateBullet(activeBullet);
            global.skill += 9;
            global.AddSkill();
            global.POWER = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            global.POWER = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (Input.GetAxisRaw("Vertical") > 0 && global.skill > 0)
            {
                ShootBulletShield();
            }else if (activeBullet > 0)
            {
                Shoot();
            }
        }
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0, 0);

        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.MovePosition(rb.position + move);
    }

    public void TakeDamage()
    {
        global.MinusLive();
        Die();
    }

    void Shoot()
    {
        audioSource.clip = shootSound;
        audioSource.Play();

        activeBullet --;
        global.UpdateBullet(activeBullet);
        if (global.POWER) activeBullet = 200;
        Vector3 spawnPos = gameObject.transform.position;
        spawnPos.z += 1.5f;

        GameObject bullet = Instantiate(playerBullet, spawnPos, Quaternion.identity) as GameObject;
    }

    void ShootBulletShield()
    {

        audioSource.clip = shootSound;
        audioSource.Play();

        global.MinusSkill();
        Vector3 spawnPos = gameObject.transform.position;
        int angleStep = 10;
        float radius = 3f;

        for(int i = 10; i <= 170; i+= angleStep)
        {
            float radian = Mathf.Deg2Rad * i;

            float x = Mathf.Cos(radian) * radius;
            float z = Mathf.Sin(radian) * radius;

            Vector3 bulletPosition = new Vector3(spawnPos.x + x, spawnPos.y, spawnPos.z + z);

            float speed = 500f;
            Vector3 newThrust = new Vector3(x * speed, 0, z * speed);
            Quaternion rotation = Quaternion.LookRotation(new Vector3(x, 0, z).normalized, Vector3.up);
            GameObject bullet = Instantiate(playerBullet, bulletPosition, rotation);

            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            bulletScript.isAlive = true;
            bulletScript.thrust = newThrust;

        }

    }

    public void AbsorbBulletRainSkill()
    {
        Debug.Log("addddd skilll");
        global.AddSkill();
    }

    public void AbsorbLife()
    {
        Debug.Log("addddd life");
        global.AddLife(1);
    }

    void Die()
    {
        audioSource.clip = deathSound;
        audioSource.Play();

        // Explosion Effect
        Instantiate(deathExplosion, gameObject.transform.position, Quaternion.AngleAxis(-90, Vector3.right));
        StartCoroutine(BlinkAndDie(1.5f, 0.2f));
        if (global.lives == 0) { Destroy(gameObject); }

    }

    IEnumerator BlinkAndDie(float duration, float blinkInterval)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        float timer = 0f;

        while (timer < duration)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        renderer.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PlayerBullet"))
        {
            Destroy(collision.collider.gameObject);
            activeBullet++;
            global.UpdateBullet(activeBullet);
        }
    }

}
