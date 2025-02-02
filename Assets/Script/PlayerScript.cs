using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool hasActiveBullet;
    public GameObject playerBullet;
    public float speed;

    private Gobal global;

    public bool invincible = false;
    // Start is called before the first frame update
    void Start()
    {
        hasActiveBullet = true;

        global = GameObject.Find("Global").GetComponent<Gobal>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && hasActiveBullet)
        {
            Debug.Log("shoot");
            Shoot();
        }
        float move = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        transform.position += new Vector3(move, 0, 0);
    }

    public void TakeDamage()
    {
        global.MinusLive();
    }

    void Shoot()
    {
        hasActiveBullet = false;
        if (invincible) hasActiveBullet = true;
        Vector3 spawnPos = gameObject.transform.position;
        spawnPos.z += 1.5f;

        GameObject bullet = Instantiate(playerBullet, spawnPos, Quaternion.identity) as GameObject;
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
