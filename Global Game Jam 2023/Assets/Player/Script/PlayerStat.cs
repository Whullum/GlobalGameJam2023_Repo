using System.Collections;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField]
    private PlayerSounds playerSounds;
    private int playerHealth;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Color hitColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        playerHealth = maxHealth;
    }

    public void DealDamage(int damageAmount)
    {
        Debug.Log("Hit");
        playerHealth -= damageAmount;
        playerSounds.playerDamagedSound.Post(gameObject);
        UI_PlayerDungeon.Instance.UpdateHealth(0, playerHealth, maxHealth);
        StartCoroutine(HitFeedback());

        if (playerHealth <= (maxHealth / 2))
        {
            MusicManager.instance.dangerState.SetValue();
        }

        if (playerHealth <= 0)
        {
            MusicManager.instance.deathState.SetValue();
            LevelExit.GameOver?.Invoke();
        }
    }

    /// <summary>
    /// Upgrades the player health.
    /// </summary>
    /// <param name="healthUpgrade">Aditional health.</param>
    public void UpgradeHealth(int healthUpgrade)
    {
        maxHealth += healthUpgrade;
        playerHealth = maxHealth;

        UI_PlayerDungeon.Instance.UpdateHealth(0, playerHealth, maxHealth);
    }

    private IEnumerator HitFeedback()
    {
        spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;
    }
}
