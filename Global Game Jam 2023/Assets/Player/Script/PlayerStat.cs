using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField]
    private PlayerSounds playerSounds;
    private int playerHealth;

    [SerializeField] private int maxHealth = 100;

    void Start()
    {
        playerHealth = maxHealth;
    }

    public void DealDamage(int damageAmount)
    {
        
        playerHealth -= damageAmount;
        playerSounds.playerDamagedSound.Post(gameObject);
        UI_PlayerDungeon.Instance.UpdateHealth(0, playerHealth, maxHealth);

        if (playerHealth <= (maxHealth / 25))
        {
            MusicManager.instance.dangerState.SetValue();
        }

        if (playerHealth <= 0)
        {
            MusicManager.instance.deathState.SetValue();
        }

        if (playerHealth < 1)
            LevelExit.GameOver?.Invoke();
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
}
