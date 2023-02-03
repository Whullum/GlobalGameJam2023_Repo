using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.activeSelf != false && collision.name == "Player")
        {
            Debug.Log("Hit");
            collision.gameObject.transform.GetComponent<PlayerStat>().DealDamage(25f);
            Debug.Log(collision.gameObject.GetComponent<PlayerStat>().GetHealth());
        }
    }
}
