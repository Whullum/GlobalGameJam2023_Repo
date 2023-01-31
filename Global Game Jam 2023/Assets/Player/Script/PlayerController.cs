using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private float vertical;
    private float horizontal;
    private float speed;
    public float walkSpeed = 5f;
    public float runSpeed = 7f;
    public float dodgeSpeed = 25f;
    public float dodgeCooldown = 10f;
    private float timeLimit = 0f;
    private float timeDiff = 0f;
    private bool dodgeInactive = false;
    private GameObject HurtBox;

    Vector2 movement;
    [SerializeField] private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        //PlayerSpawn();
        HurtBox = GameObject.Find("HurBox");
    }

    // Update is called once per frame
    void Update()
    {
        //Handling Input
        //Recieves input for each axis
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        

    }

    private void FixedUpdate()
    {
        PlayerMovement();
        PlayerDodge();
    }

    void PlayerSpawn()
    {
        Grid tiles = GameObject.Find("LevelGenerator").GetComponentInChildren<Grid>();
        Debug.Log(tiles);
    }

    //Movement
    void PlayerMovement()
    {
        
        //Moves player from the current position of their RigidBody2D to the next position specified by movement
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        //HurtBox.transform.RotateAroundLocal(rb.position, 90f);
        //Debug.Log(rb.position);

        //Debug.Log(Input.GetKey(KeyCode.LeftShift));
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
        //Debug.Log(speed);
    }

    //Dodge
    void PlayerDodge()
    {
        
        if (Input.GetAxis("Jump") == 1 && Time.time > timeLimit)
        {
            //Debug.Log(Input.GetAxis("Jump"));
            //Debug.Log(Physics2D.Raycast(rb.position, transform.TransformDirection(rb.position + movement), 5));
            //Debug.DrawLine(rb.position, transform.TransformDirection(rb.position + movement), Color.red, 3f);

            rb.MovePosition(rb.position + movement * dodgeSpeed * Time.fixedDeltaTime);
            
            timeLimit = Time.time + dodgeCooldown;
            dodgeInactive = true;
            Debug.Log(timeLimit);
            
        }
        else if(Time.time < timeLimit && dodgeInactive)
        {
            Debug.Log(Time.time);
        }
        else if(dodgeInactive)
        {
            Debug.Log("Cooldown Ended");
            dodgeInactive = false;
        }
            
    }

    void PlayerAttack()
    {

    }
}
