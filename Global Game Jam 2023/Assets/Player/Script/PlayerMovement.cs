using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float vertical;
    private float horizontal;
    private float speed;
    public float walkSpeed = 8f;
    public float runSpeed = 10f;
    private bool isMoving = false;
    Vector2 movement;
    [SerializeField] private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
        //Movement
        //Moves player from the current position of their RigidBody2D to the next position specified by movement
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        //Debug.Log(rb.position);

        Debug.Log(Input.GetKey(KeyCode.LeftShift));
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
        Debug.Log(speed);
    }
}
