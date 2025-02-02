using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOScript : AlienScript
{
    private void Start()
    {
        global = GameObject.Find("Global").GetComponent<Gobal>();
        Move(Vector3.right * 200f);
    }
    public override void Move(Vector3 dir)
    {
        this.GetComponent<Rigidbody>().AddRelativeForce(dir);
        
    }
    
}
