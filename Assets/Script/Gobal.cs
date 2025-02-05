using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gobal : MonoBehaviour
{
    public TMP_Text scoreBoard;
    public TMP_Text livesBoard;
    public TMP_Text skillBoard;
    public TMP_Text bulletBoard;
    public TMP_Text enemyBoard;
    private int score = 0;
    public int lives = 3;
    public int skill = 3;
    public int curLevel = 0;

    public GameObject alien;
    public GameObject UFO;
    public int curRow = 3;

    private int curAlienNum = 0;

    public bool POWER = false;

    private AudioSource audioSource;
    public AudioClip gainSkillSound;
    public AudioClip gainLifeSound;
    public AudioClip gainPointSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();

        scoreBoard.text = score.ToString();

        // TODO change to ship sprite
        livesBoard.text = lives.ToString();

        skillBoard.text = skill.ToString();

        enemyBoard.text = curAlienNum.ToString();

        //Update Bullet num
        UpdateBullet(GameObject.Find("Player").GetComponent<PlayerScript>().activeBullet);

        // Spawn the aliens
        SpawnAlien();

        // Spawn UFO
        StartCoroutine(SpawnUFO());


        // sound for alien
    }

    public void SpawnAlien()
    {
        // Spawn the aliens
        for (int r = 0; r < (curRow + curLevel); r++)
        {
            for (int c = -6; c < 5; c++)
            {
                curAlienNum++;
                Vector3 spawnPos = new Vector3(c * 2f, 0, 8.0f - r * 2f);
                GameObject alienObj = Instantiate(alien, spawnPos, Quaternion.identity) as GameObject;
                alienObj.GetComponent<AlienScript>().SetUUID(c + r * 11);
                enemyBoard.text = curAlienNum.ToString();
            }
        }
    }

    public void UpdateBullet(int bullet)
    {
        bulletBoard.text = bullet.ToString();
    }

    IEnumerator SpawnUFO()
    {
        GameObject spawnedUFO = null;
        while (!spawnedUFO)
        {
            //if (spawnedUFO != null) Destroy(spawnedUFO);
            yield return new WaitForSeconds(12f);

            Vector3 spawnPosition = new Vector3(-20, 0, 10);
            spawnedUFO = Instantiate(UFO, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(11.9f);
            Destroy(spawnedUFO);
            spawnedUFO = null;
        }
    }
    public void AddLife(int num)
    {
        audioSource.clip = gainLifeSound;
        audioSource.Play();

        lives += num;
        livesBoard.text = lives.ToString();
    }

    public void AddScore(int points)
    {
        audioSource.clip = gainPointSound;
        audioSource.Play();

        score += points;
        scoreBoard.text = score.ToString();
    }

    public void AddSkill()
    {
        Debug.Log("Add skilll here");

        audioSource.clip = gainSkillSound;
        audioSource.Play();

        skill++;
        skillBoard.text = skill.ToString();
    }
    public void MinusSkill()
    {
        skill--;
        skillBoard.text = skill.ToString();
    }

    public void KillOneAlien()
    {
        curAlienNum--;
        enemyBoard.text = curAlienNum.ToString();
        if (curAlienNum <= 0)
        {
            // clean all zombie alien
            GameObject[] zombies = GameObject.FindGameObjectsWithTag("Alien");
            foreach (GameObject zombie in zombies)
            {
                Destroy(zombie);
            }
            // Respawn Alien
            curLevel++;
            curAlienNum = 0;
            enemyBoard.text = curAlienNum.ToString();
            SpawnAlien();
        }
    }

    public void MinusLive()
    {
        lives--;

        // TODO change to ship sprite
        livesBoard.text = lives.ToString();
        if(lives == 0)
        {

            SceneManager.LoadScene("EndScene");
            Debug.Log("Game Over");

            GameManager.Instance.SaveScore();
        }
    }


}
