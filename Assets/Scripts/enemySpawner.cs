using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    
    //We want the speed to go from 2 to 8 in 60s, so I guess... 
    private float speedmultipler = 0.1f;
    
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (enemyMovement.doNotMove) return;
        enemyMovement.enemySpeed += Time.deltaTime * speedmultipler;

    }
}
