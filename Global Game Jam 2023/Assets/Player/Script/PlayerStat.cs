using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHealth = 100f;
    private float playerHealth;
    void Start()
    {
        playerHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth < 1)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void DealDamage(float damageAmount)
    {
        playerHealth -= damageAmount;

    }
    public float GetHealth()
    {
        return playerHealth;
    }
}
