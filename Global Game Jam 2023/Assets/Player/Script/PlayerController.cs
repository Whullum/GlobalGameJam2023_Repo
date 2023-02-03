using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float vertical;
    private float horizontal;
    private float speed;
    public float walkSpeed = 5f;
    public float runSpeed = 7f;
    public float dodgeSpeed = 25f;
    public float dodgeCooldown = 10f;
    public float attackTime = 0.5f;
    private float timeLimit = 0f;
    private float attacktimeLimit = 0f;
    private float timeDiff = 0f;
    private bool dodgeInactive = false;
    private bool attackActive = false;
    private GameObject HurtBox;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerStat playerHealth;
    private PlayerCombat playerCombat;
    private Vector3 hurtBoxDistance;

    Vector2 movement;
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerStat>();
        playerCombat = GetComponent<PlayerCombat>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //PlayerSpawn();
        HurtBox = gameObject.transform.GetChild(0).gameObject;
        //hurtBoxDistance = HurtBox.transform.position - gameObject.transform.position;

        //hurtBoxDistance.x = Math.Abs(hurtBoxDistance.x);
        //hurtBoxDistance.y = Math.Abs(hurtBoxDistance.y);
        //hurtBoxDistance.z = Math.Abs(hurtBoxDistance.z);
        //Debug.Log(hurtBoxDistance);
        LoadPlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        //Handling Input
        //Recieves input for each axis
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        if (movement != Vector2.zero)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);

        // Flip sprite renderer depending on movement direction
        if (movement.x > 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        PlayerDodge();
        PlayerAttack();
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


        //Debug.Log(movement.y);
        if (movement.y != 0)
        {
            HurtBox.transform.rotation = Quaternion.Euler(0, 0, 90);

        }
        else if (movement.x != 0)
        {
            HurtBox.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (movement.x > 0)
        {

            HurtBox.transform.position = new Vector3(gameObject.transform.position.x + 0.39f, gameObject.transform.position.y, HurtBox.transform.position.z);
        }
        else if (movement.x < 0)
        {
            HurtBox.transform.position = new Vector3(gameObject.transform.position.x - 0.39f, gameObject.transform.position.y, HurtBox.transform.position.z);
        }

        if (movement.y > 0)
        {
            HurtBox.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, HurtBox.transform.position.z);
        }
        else if (movement.y < 0)
        {
            HurtBox.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f, HurtBox.transform.position.z);
        }



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
            Debug.Log("Jump");
            //Debug.Log(Input.GetAxis("Jump"));
            //Debug.Log(Physics2D.Raycast(rb.position, transform.TransformDirection(rb.position + movement), 5));
            //Debug.DrawLine(rb.position, transform.TransformDirection(rb.position + movement), Color.red, 3f);

            rb.MovePosition(rb.position + movement * dodgeSpeed * Time.fixedDeltaTime);

            timeLimit = Time.time + dodgeCooldown;
            dodgeInactive = true;
            //Debug.Log(timeLimit);

            animator.SetTrigger("dodge");

        }
        else if (Time.time < timeLimit && dodgeInactive)
        {
            //Debug.Log(Time.time);
        }
        else if (dodgeInactive)
        {
            Debug.Log("Cooldown Ended");
            dodgeInactive = false;
            timeLimit = 0f;
        }

    }

    void PlayerAttack()
    {
        //Debug.Log(Input.GetKey(KeyCode.Mouse0));

        if (Input.GetKey(KeyCode.Mouse0) && Time.time > attacktimeLimit)
        {

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

    /// <summary>
    /// Loads from the PlayerManager all the upgrades, abilities and weapons bought.
    /// </summary>
    private void LoadPlayerStats()
    {
        Debug.Log(PlayerManager.DashUnlocked);
        if (PlayerManager.DashUnlocked)
            gameObject.AddComponent<DashAbility>();
        if (PlayerManager.ReflectUnlocked) 
            gameObject.AddComponent<ReflectAbility>();

        playerCombat.UpgradeAttack(PlayerManager.AttackStat);

        walkSpeed += PlayerManager.MovementStat;
        runSpeed  += PlayerManager.MovementStat;

        playerHealth.UpgradeHealth(PlayerManager.HealthStat);
    }
}
