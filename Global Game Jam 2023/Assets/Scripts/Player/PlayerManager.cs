using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string DashCost { get { return dashAbility.Cost.ToString(); } }
    public string ReflectCost { get { return reflectAbility.Cost.ToString(); } }
    public string SwordCost { get { return sword.Cost.ToString(); } }
    public string BowCost { get { return bow.Cost.ToString(); } }
    public static int AttackStat { get; private set; } = 0;
    public static int MovementStat { get; private set; } = 0;
    public static int HealthStat { get; private set; } = 0;
    public static bool DashUnlocked { get; private set; }
    public static bool ReflectUnlocked { get; private set; }
    public static bool SwordUnlocked { get; private set; }
    public static bool BowUnlocked { get; private set; }
    private UI_UpgradesMenu upgradesUI;
    private UI_HubShopMenu hubShopUI;

    [Header("Player Upgrades")]
    [Tooltip("ScriptableObject with the attack upgrades.")]
    [SerializeField] private PlayerUpgrade attackUpgrade;
    [Tooltip("ScriptableObject with the movement upgrades.")]
    [SerializeField] private PlayerUpgrade movementUpgrade;
    [Tooltip("ScriptableObject with the health upgrades.")]
    [SerializeField] private PlayerUpgrade healthUpgrade;
    [Header("Player Abilities")]
    [SerializeField] private Ability dashAbility;
    [SerializeField] private Ability reflectAbility;
    [Header("Player Weapons")]
    [SerializeField] private Weapon sword;
    [SerializeField] private Weapon bow;

    private void Awake()
    {
        upgradesUI = FindObjectOfType<UI_UpgradesMenu>(true);
        hubShopUI = FindObjectOfType<UI_HubShopMenu>(true);
        LoadUpgrades();
        DashUnlocked = dashAbility.Unlocked;
        ReflectUnlocked = reflectAbility.Unlocked;
    }

    /// <summary>
    /// Adds DashAbility component to the player.
    /// </summary>
    public bool UnlockDashAbility()
    {
        if (DashUnlocked) return true;
        if (SeedWallet.SpendSeeds(dashAbility.Cost))
        {
            DashUnlocked = true;
            dashAbility.Unlocked = true;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds ReflectAbility component to the player.
    /// </summary>
    public bool UnlockReflectAbility()
    {
        if (ReflectUnlocked) return true;
        if (SeedWallet.SpendSeeds(reflectAbility.Cost))
        {
            ReflectUnlocked = true;
            reflectAbility.Unlocked = true;
            return true;
        }
        return false;
    }

    public bool UnlockSword()
    {
        if (SwordUnlocked) return true;
        if (SeedWallet.SpendSeeds(sword.Cost))
        {
            SwordUnlocked = true;
            sword.Unlocked = true;
            return true;
        }
        return false;
    }

    public bool UnlockBow()
    {
        if (BowUnlocked) return true;
        if (SeedWallet.SpendSeeds(bow.Cost))
        {
            BowUnlocked = true;
            bow.Unlocked = true;
            return true;
        }
        return false;
    }

    public void Upgrade(UpgradeType upgradeType)
    {
        PlayerUpgrade playerUpgrades = null;

        switch (upgradeType)
        {
            case UpgradeType.Attack:
                playerUpgrades = attackUpgrade;
                break;
            case UpgradeType.Movement:
                playerUpgrades = movementUpgrade;
                break;
            case UpgradeType.Health:
                playerUpgrades = healthUpgrade;
                break;
        }

        // All upgrades already bought
        if (playerUpgrades.AllUpgradesUnlocked) return;
        // Cannot afford upgrade
        if (!SeedWallet.SpendSeeds(playerUpgrades.NextUpgrade.Cost)) return;

        playerUpgrades.NextUpgrade.Unlocked = true;
        playerUpgrades.CurrentUpgrade = playerUpgrades.NextUpgrade;

        LoadUpgrades();

        // We need to check if all the upgrades are bought, so we can display that inside the UI.
        bool allUpgradesUnlocked = true;
        foreach (Upgrade upgrade in playerUpgrades.Upgrades)
        {
            if (!upgrade.Unlocked)
                allUpgradesUnlocked = false;
        }

        playerUpgrades.AllUpgradesUnlocked = allUpgradesUnlocked;

        UpdateUpgradesUI();
    }

    private void LoadUpgrades()
    {
        Upgrade lastUpgrade = null;
        HealthStat = 0;
        MovementStat = 0;
        AttackStat = 0;

        foreach (Upgrade upgrade in attackUpgrade.Upgrades)
        {
            if (upgrade.Unlocked)
            {
                attackUpgrade.CurrentUpgrade = upgrade;
                AttackStat += upgrade.Value;
            }

            if (attackUpgrade.AllUpgradesUnlocked)
            {
                attackUpgrade.CurrentUpgrade = attackUpgrade.Upgrades[attackUpgrade.Upgrades.Length - 1];
                attackUpgrade.NextUpgrade = attackUpgrade.Upgrades[attackUpgrade.Upgrades.Length - 1];
                break;
            }

            if (!upgrade.Unlocked)
            {
                attackUpgrade.NextUpgrade = upgrade;
                break;
            }
        }

        if (attackUpgrade.CurrentUpgrade == null)
            attackUpgrade.CurrentUpgrade = new();

        lastUpgrade = null;

        foreach (Upgrade upgrade in movementUpgrade.Upgrades)
        {
            lastUpgrade = upgrade;

            if (upgrade.Unlocked)
            {
                movementUpgrade.CurrentUpgrade = upgrade;
                MovementStat += upgrade.Value;
            }

            if (movementUpgrade.AllUpgradesUnlocked)
            {
                movementUpgrade.CurrentUpgrade = movementUpgrade.Upgrades[movementUpgrade.Upgrades.Length - 1];
                movementUpgrade.NextUpgrade = movementUpgrade.Upgrades[movementUpgrade.Upgrades.Length - 1];
                break;
            }

            if (!upgrade.Unlocked)
            {
                movementUpgrade.NextUpgrade = lastUpgrade;
                break;
            }
        }

        if (movementUpgrade.CurrentUpgrade == null)
            movementUpgrade.CurrentUpgrade = new();

        lastUpgrade = null;

        foreach (Upgrade upgrade in healthUpgrade.Upgrades)
        {
            lastUpgrade = upgrade;

            if (upgrade.Unlocked)
            {
                healthUpgrade.CurrentUpgrade = upgrade;
                HealthStat += upgrade.Value;
            }

            if (healthUpgrade.AllUpgradesUnlocked)
            {
                healthUpgrade.CurrentUpgrade = healthUpgrade.Upgrades[healthUpgrade.Upgrades.Length - 1];
                healthUpgrade.NextUpgrade = healthUpgrade.Upgrades[healthUpgrade.Upgrades.Length - 1];
                break;
            }

            if (!upgrade.Unlocked)
            {
                healthUpgrade.NextUpgrade = lastUpgrade;
                break;
            }
        }
        if (healthUpgrade.CurrentUpgrade == null)
            healthUpgrade.CurrentUpgrade = new();
    }

    public void UpdateUpgradesUI()
    {
        hubShopUI.UpdateSeedsCounter(SeedWallet.Seeds);
        upgradesUI.UpdateValues(upgradesUI.damageCurrentText, upgradesUI.damageUpgradeText, attackUpgrade.CurrentUpgrade.Value, attackUpgrade.NextUpgrade.Value);
        upgradesUI.UpdateValues(upgradesUI.moveSpeedCurrentText, upgradesUI.moveSpeedUpgradeText, movementUpgrade.CurrentUpgrade.Value, movementUpgrade.NextUpgrade.Value);
        upgradesUI.UpdateValues(upgradesUI.healthCurrentText, upgradesUI.healthUpgradeText, healthUpgrade.CurrentUpgrade.Value, healthUpgrade.NextUpgrade.Value);
        upgradesUI.UpdateCost(upgradesUI.damageCostText, attackUpgrade.NextUpgrade.Cost);
        upgradesUI.UpdateCost(upgradesUI.healthCostText, healthUpgrade.NextUpgrade.Cost);
        upgradesUI.UpdateCost(upgradesUI.moveSpeedCostText, movementUpgrade.NextUpgrade.Cost);
    }
}
