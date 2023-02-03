using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    private static Transform playerTarget;
    private bool playerInRange;
    private bool playerOnSight;
    private float nextFire;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [Range(0.5f, 60)]
    [SerializeField] private float rotationSpeed = 1f;
    [Range(0.1f, 5)]
    [SerializeField] private float searchTick = 0.5f;
    [Range(1f, 50f)]
    [SerializeField] private float range = 5f;
    [Range(0.1f, 30)]
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadTime;
    [Range(1f, 60)]
    [SerializeField] private float attackAngle = 20f;
    [SerializeField] private int magazineSize;
    [SerializeField] private bool needReload;

    private void Awake()
    {
        if (playerTarget == null) playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        InvokeRepeating("SearchPlayer", searchTick, searchTick);
    }

    private void Update()
    {
        RotateTowardsPlayer();
        ShootProjectile();

        nextFire -= Time.deltaTime * fireRate;
    }

    private void RotateTowardsPlayer()
    {
        if (!playerInRange) return;

        Vector3 playerDirection = playerTarget.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, range, ~LayerMask.GetMask("LevelBoundary", "Roots", "ReflectShield"));

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            float rotation = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
            Quaternion lookRotation = Quaternion.AngleAxis(rotation, Vector3.forward);

            playerOnSight = true;
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        else
            playerOnSight = false;
    }

    private void ShootProjectile()
    {
        if (nextFire <= 0 && playerOnSight)
        {
            float angleToPlayer = Vector2.Angle(transform.forward, playerTarget.position);
            if (angleToPlayer <= attackAngle)
            {
                Instantiate(projectilePrefab, shootPoint.position, transform.rotation);
                nextFire = 1;
            }
        }
    }

    private void SearchPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer <= range)
            playerInRange = true;
        else
        {
            playerOnSight = false;
            playerInRange = false;
        }
    }
}
