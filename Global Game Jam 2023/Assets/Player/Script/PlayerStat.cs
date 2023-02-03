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

        if (playerHealth <= (maxHealth / 10))
        {
            MusicManager.instance.dangerState.SetValue();
        }

        if (playerHealth <= 0)
        {
            MusicManager.instance.deathState.SetValue();
        if (playerHealth < 1)
        {
            LevelLoader.RestartGame();
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
    }
}
