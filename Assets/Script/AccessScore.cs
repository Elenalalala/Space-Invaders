using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccessScore : MonoBehaviour
{
    public TMP_Text scoreBoard;
    // Start is called before the first frame update
    void Start()
    {
        scoreBoard.text = GameManager.Instance.curScore;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
