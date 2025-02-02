using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    private int health;
    public List<Material> damageMaterials;

    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        health = 4;
        rend = this.gameObject.GetComponent<Renderer>();
        rend.material = damageMaterials[health - 1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        health--;
        if (health == 0) {
            Destroy(gameObject);
        }
        else
        {
            rend.material = damageMaterials[health - 1];
        }

    }
}
