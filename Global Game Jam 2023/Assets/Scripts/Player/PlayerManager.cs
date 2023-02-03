using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string DashCost { get { return dashAbility.Cost.ToString(); } }
    public string ReflectCost { get { return reflectAbility.Cost.ToString(); } }
    public static int AttackStat { get; private set; } = 0;
    public static int movementStat { get; private set; } = 0;
    public static int healthStat { get; private set; } = 0;
    public static bool DashUnlocked { get; private set; }
    public static bool ReflectUnlocked { get; private set; }
    private UI_UpgradesMenu upgradesUI;
    private UI_AbilitiesMenu abilitiesUI;
    private UI_HubShopMenu hubShopUI;
    private static GameObject player;


    [SerializeField] private GameObject playerPrefab;
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

    private void Awake()
    {
        upgradesUI = FindObjectOfType<UI_UpgradesMenu>(true);
        abilitiesUI = FindObjectOfType<UI_AbilitiesMenu>(true);
        hubShopUI = FindObjectOfType<UI_HubShopMenu>(true);
        SeedWallet.CollectSeeds(100000);
        LoadUpgrades();
        DashUnlocked = dashAbility.Unlocked;
        ReflectUnlocked = reflectAbility.Unlocked;
    }

    private void Start()
    {
        UpdateUpgradesUI();
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

        foreach (Upgrade upgrade in attackUpgrade.Upgrades)
        {
            if (upgrade.Unlocked)
            {
                attackUpgrade.CurrentUpgrade = upgrade;
                AttackStat += upgrade.Value;
            }

            if (!upgrade.Unlocked)
            {
                attackUpgrade.NextUpgrade = upgrade;
                break;
            }
        }

        lastUpgrade = null;

        foreach (Upgrade upgrade in movementUpgrade.Upgrades)
        {
            lastUpgrade = upgrade;

            if (upgrade.Unlocked)
            {
                movementUpgrade.CurrentUpgrade = upgrade;
                movementStat += upgrade.Value;
            }

            if (!upgrade.Unlocked)
            {
                movementUpgrade.NextUpgrade = lastUpgrade;
                break;
            }
        }

        lastUpgrade = null;

        foreach (Upgrade upgrade in healthUpgrade.Upgrades)
        {
            lastUpgrade = upgrade;

            if (upgrade.Unlocked)
            {
                healthUpgrade.CurrentUpgrade = upgrade;
                healthStat += upgrade.Value;
            }

            if (!upgrade.Unlocked)
            {
                healthUpgrade.NextUpgrade = lastUpgrade;
                break;
            }
        }
    }

    private void UpdateUpgradesUI()
    {
        hubShopUI.UpdateSeedsCounter(SeedWallet.Seeds);
        upgradesUI.UpdateValues(upgradesUI.damageCurrentText, upgradesUI.damageUpgradeText, attackUpgrade.CurrentUpgrade.Value, attackUpgrade.NextUpgrade.Value);
        upgradesUI.UpdateValues(upgradesUI.moveSpeedCurrentText, upgradesUI.moveSpeedUpgradeText, movementUpgrade.CurrentUpgrade.Value, movementUpgrade.NextUpgrade.Value);
        upgradesUI.UpdateValues(upgradesUI.healthCurrentText, upgradesUI.healthUpgradeText, healthUpgrade.CurrentUpgrade.Value, healthUpgrade.NextUpgrade.Value);
        upgradesUI.UpdateCost(upgradesUI.damageCostText, attackUpgrade.NextUpgrade.Cost);
        upgradesUI.UpdateCost(upgradesUI.healthCostText, healthUpgrade.NextUpgrade.Cost);
        upgradesUI.UpdateCost(upgradesUI.moveSpeedCostText, movementUpgrade.NextUpgrade.Cost);
    }

    private void OnDisable()
    {
        attackUpgrade.CurrentUpgrade = null;
        attackUpgrade.NextUpgrade = null;
        healthUpgrade.CurrentUpgrade = null;
        healthUpgrade.NextUpgrade = null;
        movementUpgrade.CurrentUpgrade = null;
        movementUpgrade.NextUpgrade = null;
    }
}
