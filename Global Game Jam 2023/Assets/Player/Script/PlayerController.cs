using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool BowEquiped { get; private set; }
    private float vertical;
    private float horizontal;

    public float walkSpeed = 5f;
    public float runSpeed = 7f;
    private float speed;
    public float attackTime = 0.5f;
    private float timeLimit = 0f;
    private float attacktimeLimit = 0f;
    private float lastMovementDirection = 0;
    private bool swordEquiped;
    private bool attackActive = false;
    private GameObject HurtBox;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerStat playerHealth;
    private PlayerCombat playerCombat;
    private Vector3 hurtBoxDistance;

    Vector2 movementDirection;
    [SerializeField] private SpriteRenderer swordRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Sprite swordWeapon;
    [SerializeField] private Sprite bowWeapon;
    [SerializeField] private Sprite noWeapon;
    [SerializeField] private Sprite reflectAbility;
    [SerializeField] private Sprite dodgeAbility;
    [SerializeField]
    private PlayerSounds playerSounds;

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
        speed = walkSpeed;
        LoadPlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        //Handling Input
        //Recieves input for each axis
        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.y = Input.GetAxis("Vertical");

        if (movementDirection != Vector2.zero)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);

        if (movementDirection.x != 0)
            lastMovementDirection = movementDirection.x;
        // Flip sprite renderer depending on movement direction
        if (lastMovementDirection > 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame.instance.OpenPauseMenu();
        }

        EquipWeapon();
    }

    private void FixedUpdate()
    {
        PlayerMovement();

        PlayerAttack();
    }

    private void EquipWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && PlayerManager.SwordUnlocked)
        {
            swordEquiped = true;
            BowEquiped = false;
            UI_PlayerDungeon.Instance.SetEquipedWeapon(swordWeapon);
            UI_PlayerDungeon.Instance.ChangeWewaponText("Sword");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && PlayerManager.BowUnlocked)
        {
            swordEquiped = false;
            BowEquiped = true;
            UI_PlayerDungeon.Instance.SetEquipedWeapon(bowWeapon);
            UI_PlayerDungeon.Instance.ChangeWewaponText("Bow");
        }

        if (Input.GetKeyDown(KeyCode.Q) && PlayerManager.ReflectUnlocked)
        {
            UI_PlayerDungeon.Instance.SetActiveAbility(reflectAbility);
        }

        if (Input.GetKeyDown(KeyCode.Space) && PlayerManager.DashUnlocked)
        {
            UI_PlayerDungeon.Instance.SetActiveAbility(dodgeAbility);
        }
    }

    //Movement
    void PlayerMovement()
    {

        //Moves player from the current position of their RigidBody2D to the next position specified by movement
        rb.MovePosition(rb.position + movementDirection * speed * Time.fixedDeltaTime);


        //Debug.Log(movement.y);
        if (movementDirection.y != 0)
        {
            HurtBox.transform.rotation = Quaternion.Euler(0, 0, 90);

        }
        else if (movementDirection.x != 0)
        {
            HurtBox.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (movementDirection.x > 0)
        {
            swordRenderer.flipX = true;
            HurtBox.transform.position = new Vector3(gameObject.transform.position.x + 0.39f, gameObject.transform.position.y, HurtBox.transform.position.z);
        }
        else if (movementDirection.x < 0)
        {
            swordRenderer.flipX = false;
            HurtBox.transform.position = new Vector3(gameObject.transform.position.x - 0.39f, gameObject.transform.position.y, HurtBox.transform.position.z);
        }

        if (movementDirection.y > 0)
        {
            swordRenderer.flipY = true;
            swordRenderer.flipX = true;
            HurtBox.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, HurtBox.transform.position.z);
        }
        else if (movementDirection.y < 0)
        {
            swordRenderer.flipY = false;
            swordRenderer.flipX = false;
            HurtBox.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f, HurtBox.transform.position.z);
        }



        //Debug.Log(rb.position);

        //Debug.Log(Input.GetKey(KeyCode.LeftShift));
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = walkSpeed;
        }
        //Debug.Log(speed);
    }



    void PlayerAttack()
    {
        //Debug.Log(Input.GetKey(KeyCode.Mouse0));

        if (Input.GetKey(KeyCode.Mouse0) && Time.time > attacktimeLimit && PlayerManager.SwordUnlocked && swordEquiped)
        {

            HurtBox.SetActive(true);

            attacktimeLimit = Time.time + attackTime;
            attackActive = true;
            //Debug.Log(attacktimeLimit);

            playerSounds.playerAttackSound.Post(gameObject);
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
        if (PlayerManager.DashUnlocked)
        {
            gameObject.AddComponent<DodgeAbility>();
            UI_PlayerDungeon.Instance.SetActiveAbility(dodgeAbility);
            UI_PlayerDungeon.Instance.SetAbilityText("Avaliable");
        }

        if (PlayerManager.ReflectUnlocked)
        {
            gameObject.AddComponent<ReflectAbility>();
            UI_PlayerDungeon.Instance.SetActiveAbility(reflectAbility);
            UI_PlayerDungeon.Instance.SetAbilityText("Avaliable");
        }

        if (PlayerManager.BowUnlocked)
        {
            GameObject bow = Instantiate(Resources.Load<GameObject>("Player/Weapons/Bow"));
            bow.transform.parent = transform;
        }

        if (!PlayerManager.BowUnlocked && !PlayerManager.SwordUnlocked)
        {
            UI_PlayerDungeon.Instance.SetEquipedWeapon(noWeapon);
            UI_PlayerDungeon.Instance.ChangeWewaponText("No weapon");
        }

        if (PlayerManager.SwordUnlocked)
        {
            swordEquiped = true;
            BowEquiped = false;
            UI_PlayerDungeon.Instance.SetEquipedWeapon(swordWeapon);
            UI_PlayerDungeon.Instance.ChangeWewaponText("Sword");
        }
        else if (PlayerManager.BowUnlocked)
        {
            swordEquiped = false;
            BowEquiped = true;
            UI_PlayerDungeon.Instance.SetEquipedWeapon(bowWeapon);
            UI_PlayerDungeon.Instance.ChangeWewaponText("Bow");
        }


        playerCombat.UpgradeAttack(PlayerManager.AttackStat);

        walkSpeed += PlayerManager.MovementStat;
        runSpeed += PlayerManager.MovementStat;

        playerHealth.UpgradeHealth(PlayerManager.HealthStat);
    }

    public float GetTimeLimit()
    {
        return timeLimit;
    }

    public void ResetTimeLimit()
    {
        timeLimit = 0f;
    }

    public void AddToTimeLimit(float number)
    {
        timeLimit = Time.time + number;
    }

    public Animator GetAnimator()
    {
        Debug.Log("Animator Got!");
        return animator;
    }

    public Vector2 GetMovementDirection()
    {
        Debug.Log("movement got");
        return movementDirection;
    }

    public Rigidbody2D GetRigidbody()
    {
        return rb;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
