using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 m_Position;
    Behavior enemyBehavior;
    GameObject player;
    float timer;
    [SerializeField] float speed = 1.5f;
    Vector2 movementDirection;
    GameObject HurtBox;
    Rigidbody2D rb;
    private float distanceFromTarget;
    EnemyType enemyType;
    [SerializeField] float playerDetectionRange = 20f;
    void Start()
    {
        HurtBox = gameObject.transform.GetChild(0).gameObject;
        enemyBehavior = Behavior.attack;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        enemyType = GetComponent<EnemyStats>().enemyType;
    }
    enum Behavior
    {
        passive,
        chase,
        attack
    }
    // Update is called once per frame
    void Update()
    {
        movementDirection = RandomDirection();
        Vector3 target = transform.position;

        if (enemyBehavior == Behavior.passive)
        {
            
            if (transform.position != target)
                transform.position = Vector3.MoveTowards(transform.position,  target, speed*Time.deltaTime);
            else
            {
                target = transform.position + RandomDirection();
            }
            
            //Debug.Log(Physics2D.Raycast(transform.position, transform.TransformDirection(transform.position + RandomDirection()), 5));
        }
        else if(enemyBehavior == Behavior.chase)
        {

        }else if(enemyBehavior == Behavior.attack && Vector3.Distance(transform.position, player.transform.position) < playerDetectionRange)
        {
            target = player.transform.position;
            if(Vector3.Distance(transform.position, target) > 0.82)
            {
                distanceFromTarget = Vector3.Distance(transform.position, target);
                //Debug.Log(Vector3.Distance(transform.position, target));

                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
            
        }
        if(enemyType == EnemyType.melee)
        {
            Vector3 direction = transform.TransformDirection(target);
            //Debug.Log(direction);
            //target is to the right
            if (direction.x > direction.y && target.x > transform.position.x)
            {
                //Debug.Log("Right");
                HurtBox.transform.rotation = Quaternion.Euler(0, 0, 0);
                HurtBox.transform.position = new Vector3(gameObject.transform.position.x + 0.75f, gameObject.transform.position.y, HurtBox.transform.position.z);
            }
            //target is to the left
            else if (direction.x < direction.y && target.x < transform.position.x)
            {
                //Debug.Log("Left");
                HurtBox.transform.rotation = Quaternion.Euler(0, 0, 0);
                HurtBox.transform.position = new Vector3(gameObject.transform.position.x - 0.75f, gameObject.transform.position.y, HurtBox.transform.position.z);
            }

            //target is above
            else if (direction.x < direction.y && target.y > transform.position.y)
            {
                //Debug.Log("Above");
                HurtBox.transform.rotation = Quaternion.Euler(0, 0, 90);
                HurtBox.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.75f, HurtBox.transform.position.z);

            }
            //target is below
            else if (direction.x > direction.y && target.y < transform.position.y)
            {
                //Debug.Log("Below");
                HurtBox.transform.rotation = Quaternion.Euler(0, 0, 90);
                HurtBox.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.75f, HurtBox.transform.position.z);

            }
        }
        else if(enemyType == EnemyType.ranged)
        {
            
        }
        
        
    }

    private Vector3 RandomDirection()
    {
        return new Vector3(Random.Range(1f, -1f), Random.Range(1f, -1f)).normalized;
    }
    public float GetDistanceFromTarget()
    {
        return distanceFromTarget;
    }
}
