using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string curScore = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveScore()
    {
        Gobal gobal = GameObject.Find("Global").GetComponent<Gobal>();
        if(gobal != null)
        {
            curScore = gobal.scoreBoard.text;
        }
    }
}
