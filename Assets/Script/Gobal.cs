using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gobal : MonoBehaviour
{
    public TMP_Text scoreBoard;
    public TMP_Text livesBoard;
    private int score = 0;
    private int lives = 3;
    public int curLevel = 0;

    public GameObject alien;
    public GameObject UFO;
    public int curRow = 3;

    private int curAlienNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        scoreBoard.text = score.ToString();

        // TODO change to ship sprite
        livesBoard.text = lives.ToString();

        // Spawn the aliens
        SpawnAlien();

        // Spawn UFO
        StartCoroutine(SpawnUFO());
    }

    public void SpawnAlien()
    {
        // Spawn the aliens
        for (int r = 0; r < (curRow + curLevel); r++)
        {
            for (int c = -6; c < 5; c++)
            {
                curAlienNum++;
                Vector3 spawnPos = new Vector3(c * 2.5f, 0, 8.0f - r * 2.5f);
                GameObject alienObj = Instantiate(alien, spawnPos, Quaternion.identity) as GameObject;
                alienObj.GetComponent<AlienScript>().SetUUID(c + r * 11);

            }
        }
    }

    IEnumerator SpawnUFO()
    {
        GameObject spawnedUFO = null;
        while (!spawnedUFO)
        {
            //if (spawnedUFO != null) Destroy(spawnedUFO);
            yield return new WaitForSeconds(10f);

            Vector3 spawnPosition = new Vector3(-20, 0, 10);
            spawnedUFO = Instantiate(UFO, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(9.9f);
            Destroy(spawnedUFO);
            spawnedUFO = null;
        }
    }

    public void AddScore(int points)
    {
        score += points;
        scoreBoard.text = score.ToString();
    }

    public void KillOneAlien()
    {
        curAlienNum--;
        if(curAlienNum <= 0)
        {
            // clean all zombie alien
            GameObject[] zombies = GameObject.FindGameObjectsWithTag("Alien");
            foreach (GameObject zombie in zombies)
            {
                Destroy(zombie);
            }
            // Respawn Alien
            curLevel++;
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
