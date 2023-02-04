using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Rigidbody2D rBody;

    [SerializeField] private float projectileSpeed;

    private void Awake()
    {
        rBody= GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        LaunchProjectile();
    }

    private void LaunchProjectile()
    {
        rBody.AddForce(transform.right * projectileSpeed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Reflect")) return;

        GetComponent<Animator>().SetTrigger("ProjectileDestroyed");
        rBody.velocity= Vector3.zero;
        
        Debug.Log("acacac");
    }

    public void DestroyProjectile()
    {
        Debug.Log("Destroy");
        Destroy(gameObject);
    }
}
