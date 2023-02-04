using System.Collections;
using UnityEngine;

public class ReflectAbility : MonoBehaviour
{
    private SpriteRenderer reflecEffect;
    private Collider2D reflectCollider;
    private bool canActivate = true;
    private bool reflectProjectiles = false;
    private float cooldownTimer;

    [Tooltip("Total time deflecting projectiles.")]
    [SerializeField] private float activeTime = 5;
    [Tooltip("Cooldown time between activations.")]
    [SerializeField] private float cooldown = 10;

    private void Awake()
    {
        // Search for the GameObject with the particle system and collider and create new GameObject
        GameObject effect = Instantiate(Resources.Load<GameObject>("Player/Abilities/ReflectAbility"), transform.position, Quaternion.identity);
        effect.transform.parent = transform;
        reflectCollider = effect.GetComponent<Collider2D>();
        reflectCollider.enabled = false;

        reflecEffect = effect.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        UI_PlayerDungeon.Instance.ChangeAblityName("Reflect");
        UI_PlayerDungeon.Instance.SetAbilityText("Avaliable");
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
        reflectCollider.enabled = true;
        canActivate = false;
        reflecEffect.enabled = true;
        UI_PlayerDungeon.Instance.ChangeAblityName("Reflect");
        UI_PlayerDungeon.Instance.SetAbilityText("Active");

        yield return new WaitForSeconds(activeTime);

        cooldownTimer = cooldown;
        reflectProjectiles = false;
        reflectCollider.enabled = false;
        reflecEffect.enabled = false;

        while (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            UI_PlayerDungeon.Instance.ChangeAbilityCooldown(cooldownTimer);
            yield return null;
        }
        cooldownTimer = 0;
        UI_PlayerDungeon.Instance.ChangeAbilityCooldown(cooldownTimer);
        UI_PlayerDungeon.Instance.SetAbilityText("Avaliable");
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
