using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHealth = 100f;
    private float enemyHealth;
    void Start()
    {
        enemyHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth < 1)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void DealDamage(float damageAmount)
    {
        enemyHealth -= damageAmount;
        
    }
    public float GetHealth()
    {
        return enemyHealth;
    }
}
