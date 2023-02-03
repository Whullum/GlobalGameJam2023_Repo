using System.Collections;
using System.Collections.Generic;
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
        Destroy(gameObject);
    }
}
