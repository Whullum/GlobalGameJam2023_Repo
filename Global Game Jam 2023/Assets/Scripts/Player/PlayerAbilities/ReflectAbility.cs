using System.Collections;
using UnityEngine;

public class ReflectAbility : MonoBehaviour
{
    private ParticleSystem reflecEffect;
    private bool canActivate = true;
    private bool reflectProjectiles = false;

    [Tooltip("Total time deflecting projectiles.")]
    [SerializeField] private float activeTime = 3;
    [Tooltip("Cooldown time between activations.")]
    [SerializeField] private float cooldown = 10;

    private void Awake()
    {
        // Search for the GameObject with the particle system and collider and create new GameObject
        GameObject effect = Instantiate(Resources.Load<GameObject>("Player/Abilities/ReflectAbility"), transform.position, Quaternion.identity);
        effect.transform.parent = transform;

        reflecEffect = effect.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (canActivate && Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(ActivateReflect());
    }

    /// <summary>
    /// Activates to projectile reflecting ability and starts cooling down once it finishes.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActivateReflect()
    {
        if (!canActivate) yield break;

        reflectProjectiles = true;
        canActivate = false;
        reflecEffect.Play();

        yield return new WaitForSeconds(activeTime);

        reflectProjectiles = false;
        reflecEffect.Stop();

        yield return new WaitForSeconds(cooldown);

        canActivate = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!reflectProjectiles) return;

        // Get collision contact point
        ContactPoint2D contact = collision.contacts[0];
        // Get projectile rigidbody
        Rigidbody2D projectile = collision.collider.GetComponent<Rigidbody2D>();
        // Calculate reflect angle
        Vector2 reflect = Vector2.Reflect(projectile.velocity, contact.normal);
        // Calculate new look rotation
        Quaternion newRotation = Quaternion.FromToRotation(projectile.velocity, contact.normal);

        // Apply new values to the projectile
        projectile.velocity = reflect;
        projectile.transform.rotation = newRotation;
    }
}
