using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    
    // Start is called before the first frame update
    public float maxHealth = 100f;
    private float enemyHealth;
    private float attacktimeLimit = 0f;
    private bool attackActive = false;
    public float attackTime = 0.5f;
    GameObject HurtBox;
    EnemyMovement EnemyController;
    public EnemyType enemyType = EnemyType.melee;

    void Start()
    {
        enemyHealth = maxHealth;
        HurtBox = transform.GetChild(0).gameObject;
        EnemyController = GetComponent<EnemyMovement>();

        if(enemyType == EnemyType.melee)
        {
            Debug.Log("Melee");
        }else if(enemyType == EnemyType.ranged)
        {
            Debug.Log("Ranged");
            
            
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        if(enemyHealth < 1)
        {
            //Debug.Log("ded");
            
            this.gameObject.SetActive(false);
        }
        EnemyAttack();
    }

    public void DealDamage(float damageAmount)
    {
        //Debug.Log("Enemy Hit!");
        
        enemyHealth -= damageAmount;
        Debug.Log(enemyHealth);

    }
    public float GetHealth()
    {
        return enemyHealth;
    }

    void EnemyAttack()
    {
        if(enemyType == EnemyType.melee)
        { 
            //Debug.Log(EnemyController.GetDistanceFromTarget());
            if (EnemyController.GetDistanceFromTarget() < 0.85f && Time.time > attacktimeLimit)
            {
                //Debug.Log("ATTACKING");
                HurtBox.SetActive(true);

                attacktimeLimit = Time.time + attackTime;
                attackActive = true;
                //Debug.Log(attacktimeLimit);


            }
            else if (Time.time < attacktimeLimit && attackActive)
            {
                //Debug.Log(Time.time);
            }
            else if (attackActive)
            {
                Debug.Log("Cooldown Ended");
                HurtBox.SetActive(false);
                attackActive = false;
                attacktimeLimit = 0f;
            }
        }
        

    }
}
