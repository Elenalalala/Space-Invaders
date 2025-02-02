using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlienScript : MonoBehaviour
{
    public int points;
    protected Gobal global;
    public GameObject alienBullet;
    public bool hasActiveBullet;
    private GameObject bullet = null;
    [SerializeField] private int uuid;

    private float timer = 0.0f;
    private float interval = 1.0f;
    private readonly int step = 12;
    private readonly float dist = 0.5f;
    private int curStep = 0;
    private Vector3 curDir;
    private AudioSource audioSource;
    private bool isMovingDownward = false;
    private readonly int speedUpInterval = 1;
    private int speedUpCounter = 0;

    private bool isAlive;
    public Material alienGrey;
    public PhysicMaterial bouncy;
    public bool isMutatable;
    private int MutatableTimes = 1;


    private float shootTimer = 0f;
    private float nextShootTime;

    public AudioClip firstBeep;
    public AudioClip secondBeep;
    // Start is called before the first frame update
    void Awake()
    {
        global = GameObject.Find("Global").GetComponent<Gobal>();
        hasActiveBullet = true;
        curDir = Vector3.right * dist;

        if (!isAlive) isAlive = true;

        audioSource = gameObject.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                if (isMovingDownward)
                {
                    Move(Vector3.back * dist);
                    isMovingDownward = false;
                    speedUpCounter++;
                    if (interval > 0.2f && speedUpCounter % speedUpInterval == 0)
                    {
                        interval -= 0.2f;
                    }
                    else
                    {
                        interval = 0.2f;
                    }
                }
                else
                {
                    Move(curDir);
                    curStep++;

                    if (curStep > step)
                    {
                        curDir *= -1;
                        curStep = 0;
                        isMovingDownward = true;
                    }
                }

                timer = 0f;
            }

            if (uuid < 11)
            {
                shootTimer += Time.deltaTime;
                if (shootTimer >= nextShootTime)
                {
                    Shoot();
                    shootTimer = 0f;
                    nextShootTime = Random.Range(3.0f, 10.0f);
                }
            }
        }

    }

    public void SetUUID(int id)
    {
        uuid = id;
    }

    public virtual void Move(Vector3 dir)
    {
        if (firstBeep && secondBeep)
        {
            gameObject.transform.position += dir;
            if (curStep % 2 == 0)
            {
                audioSource.clip = firstBeep;
            }
            else
            {
                audioSource.clip = secondBeep;
            }
            audioSource.Play();
        }
    }

    void Shoot()
    {
        float shootChance = Random.Range(0.0f, 10.0f);
        if(shootChance < 1.0f && bullet == null)
        {
            Vector3 spawnPos = gameObject.transform.position;
            spawnPos.z -= 1.5f;

            bullet = Instantiate(alienBullet, spawnPos, Quaternion.identity);
            AlienBulletScript bulletScript = bullet.GetComponent<AlienBulletScript>();
            bulletScript.OnBulletDestroyed += HandleBulletDestroyed;
        }
    }

    private void HandleBulletDestroyed()
    {
        bullet = null;
    }

    void Die()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.mass = 0.1f;

        isAlive = false;
        gameObject.GetComponent<MeshRenderer>().material = alienGrey;
        gameObject.GetComponent<BoxCollider>().material = bouncy;

    }

    bool ReallyDie()
    {
        if (!isAlive)
        {
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }
    void Mutate()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Vector3 currentVelocity = rb.velocity;
        Vector3 offset = new Vector3(0.5f, 0, 0);
        GameObject duplicate = Instantiate(this.gameObject, gameObject.transform.position + offset, Quaternion.identity) as GameObject;
        AlienScript alienScript = duplicate.GetComponent<AlienScript>();

        alienScript.isAlive = false;
        alienScript.isMutatable = false;

        Debug.Log(alienScript.isAlive);

        duplicate.GetComponent<Rigidbody>().velocity = new Vector3(-currentVelocity.x, currentVelocity.y, currentVelocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("Player"))
        {
            SceneManager.LoadScene("EndScene");
            GameManager.Instance.SaveScore();
            Debug.Log("Game Over");
        }
        if(collider.CompareTag("PlayerBullet"))
        {

            BulletScript bullet = collider.gameObject.GetComponent<BulletScript>();
            bullet.Die();


            if (ReallyDie()) return;
            Die();
            global.KillOneAlien();
            global.AddScore(points);

        }
        if (collider.CompareTag("Boundary"))
        {
            if (isMutatable && MutatableTimes > 0)
            {
                Mutate();
                MutatableTimes--;
            }
            else
            {
                // TODO explosion? 
            }
        }


        if (collider.CompareTag("Shield"))
        {
            collider.gameObject.GetComponent<ShieldScript>().TakeDamage();
        }

        if (collider.CompareTag("Alien") && collider.gameObject.GetComponent<AlienScript>().isAlive)
        {
            collider.gameObject.GetComponent<AlienScript>().Die();
            global.KillOneAlien();
        }
    }
}
