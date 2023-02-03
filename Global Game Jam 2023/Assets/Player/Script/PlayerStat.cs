using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private int playerHealth;

    [SerializeField] private int maxHealth = 100;

    void Start()
    {
        playerHealth = maxHealth;
    }

    public void DealDamage(int damageAmount)
    {
        playerHealth -= damageAmount;
        UI_PlayerDungeon.Instance.UpdateHealth(0, playerHealth, maxHealth);

        if (playerHealth < 1)
            LevelLoader.RestartGame();
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
