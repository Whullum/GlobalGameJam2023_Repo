using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    // Start is called before the first frame update
    public float enemyHealth = 100f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth < 1)
        {
            this.gameObject.SetActive(false);
        }
    }
}
