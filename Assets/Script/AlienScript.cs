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
    private readonly int step = 7;
    private readonly float dist = 0.5f;
    private int curStep = 0;
    private Vector3 curDir;
    protected AudioSource audioSource;
    private bool isMovingDownward = false;
    private readonly int speedUpInterval = 1;
    private int speedUpCounter = 0;

    public bool isAlive;
    public Material alienGrey;
    public Material alienRed;
    public Material alienGold;
    public PhysicMaterial bouncy;
    public bool isMutatable;
    private int MutatableTimes = 1;
    public bool isZombie = false;
    private bool isVictim = false;


    private float shootTimer = 0f;
    private float nextShootTime;

    private Vector3 originalPosition;
    private bool isReturning = false;
    private bool isMutating;

    public GameObject deathExplosion;
    public AudioClip deathSound;

    [Range(0, 1)] public float lightness = 0.8f;

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
        if (gameObject.transform.position.z < -30f || gameObject.transform.position.z > 30f || gameObject.transform.position.x < -100f || gameObject.transform.position.x > 100f)
        {
            if (isVictim)
            {
                Debug.Log("victimm dieeeeee");
                global.KillOneAlien();
            }
            Destroy(gameObject);
        }
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

            shootTimer += Time.deltaTime;
            if (shootTimer >= nextShootTime)
            {
                Shoot();
                shootTimer = 0f;
                nextShootTime = Random.Range(3.0f, 10.0f);
            }

            if (!isReturning)
            {
                originalPosition = this.gameObject.transform.position;
            }
            else
            {
                float returnSpeed = 10f;
                this.gameObject.transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * returnSpeed);

                if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
                {
                    isReturning = false;
                    this.gameObject.transform.position = originalPosition;
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
        gameObject.transform.position += dir;
        originalPosition += dir;

        AudioManager.Instance.PlayMovementSound();
    }

    void Shoot()
    {
        float shootChance = Random.Range(0.0f, 10.0f);
        if (shootChance < 1.0f && bullet == null)
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

    protected void Die()
    {
        audioSource.clip = deathSound;
        audioSource.Play();
        Instantiate(deathExplosion, gameObject.transform.position, Quaternion.AngleAxis(-90, Vector3.right));

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;

        rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;

        isAlive = false;
        gameObject.GetComponent<MeshRenderer>().material = alienGrey;

        //gameObject.GetComponent<BoxCollider>().material = bouncy;

    }

    bool ReallyDie()
    {
        if (!isAlive)
        {
            Instantiate(deathExplosion, gameObject.transform.position, Quaternion.AngleAxis(-90, Vector3.right));
            global.AddScore(points);
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
        gameObject.GetComponent<MeshRenderer>().material = alienRed;
        Destroy(gameObject.GetComponent<BoxCollider>());
        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().radius = 0.9f;
        gameObject.GetComponent<SphereCollider>().material = bouncy;


        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Vector3 currentVelocity = rb.velocity;

        //rb.velocity = new Vector3(-currentVelocity.x, currentVelocity.y, currentVelocity.z);

        isZombie = true;

        this.points *= 2;

        StartCoroutine(ResetMutationCooldown());


    }

    IEnumerator ResetMutationCooldown()
    {
        yield return new WaitForSeconds(1f); // You can adjust this time to better suit gameplay
        isMutating = false;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if(collider.CompareTag("Player") && this.gameObject.GetComponent<Renderer>().sharedMaterial == alienGold)
        {
            collider.gameObject.GetComponent<PlayerScript>().AbsorbBulletRainSkill();
            Destroy(gameObject);
        }

        if ((collider.CompareTag("Player") && (isZombie || isAlive)) && !global.POWER)
        {
            SceneManager.LoadScene("EndScene");
            GameManager.Instance.SaveScore();
            Debug.Log("Game Over");
        }
        if (collider.CompareTag("PlayerBullet"))
        {

            BulletScript bullet = collider.gameObject.GetComponent<BulletScript>();
            if (bullet.isAlive)
            {
                bullet.Die();

                if (isVictim) global.KillOneAlien();

                if (ReallyDie()) return;
                Die();
                global.KillOneAlien();
                global.AddScore(points);
            }

        }
        if (collider.CompareTag("Boundary"))
        {
            int rand = Random.Range(0, 10);
            if (rand > 7)
            {
                isMutatable = false;
                //isZombie = false;
            }
            if(rand <= 3 && !isAlive && !isZombie)
            {
                isMutatable = false;
                // Become Skill
                gameObject.GetComponent<MeshRenderer>().material = alienGold;
            }
            if (isMutatable && MutatableTimes > 0 && !isZombie && !isMutating)
            {
                Mutate();
                MutatableTimes--;
            }

            if (isZombie)
            {
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                Vector3 currentVelocity = rb.velocity;
                if (currentVelocity.y < 1f)
                {
                    rb.AddForce(Vector3.forward * 10f, ForceMode.Impulse);
                }
            }
        }

        if (collider.CompareTag("Alien"))
        {
            if (collider.GetComponent<AlienScript>().isZombie && !isZombie && isMutatable && isAlive)
            {
                this.Die();
                this.Mutate();
                isVictim = true;
            }
            else
            {
                float hitOffset = 0.3f;
                Vector3 hitDirection = (transform.position - collision.transform.position).normalized;
                transform.position += hitDirection * hitOffset;

                isReturning = true;
            }

        }


        //if (collider.CompareTag("Shield"))
        //{
        //    collider.gameObject.GetComponent<ShieldScript>().TakeDamage();
        //}

        //if (collider.CompareTag("Alien") && collider.gameObject.GetComponent<AlienScript>().isAlive)
        //{
        //    collider.gameObject.GetComponent<AlienScript>().Die();
        //    global.KillOneAlien();
        //}
    }
}
