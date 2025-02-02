using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFlow : MonoBehaviour
{
    public void GoStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void GoPlayScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }

}
