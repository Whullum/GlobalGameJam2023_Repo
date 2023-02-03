using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameObject player;
    private bool dashUnlocked = false;
    private bool reflectUnlocked = false;

    [Header("Player upgrades")]
    [Tooltip("ScriptableObject with the attack upgrades.")]
    [SerializeField] private PlayerUpgrade attackUpgrade;
    [Tooltip("ScriptableObject with the movement upgrades.")]
    [SerializeField] private PlayerUpgrade movementUpgrade;
    [Tooltip("ScriptableObject with the health upgrades.")]
    [SerializeField] private PlayerUpgrade healthUpgrade;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Adds DashAbility component to the player.
    /// </summary>
    public void UnlockDashAbility()
    {
        if (dashUnlocked) return;

        player.AddComponent<DashAbility>();
    }

    /// <summary>
    /// Adds ReflectAbility component to the player.
    /// </summary>
    public void UnlockReflectAbility()
    {
        if (reflectUnlocked) return;

        player.AddComponent<ReflectAbility>();
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

        foreach (Upgrade upgrade in playerUpgrades.Upgrades)
        {
            // If this upgrade is already unlocked, continue to the next one.
            if (upgrade.Unlocked) { continue; }

            // Add attack upgrade to player and set this upgrade to be unlocked.
            upgrade.Unlocked = true;
            playerUpgrades.CurrentUpgrade = upgrade;
            break;
        }

        bool allUpgradesUnlocked = true;
        // We need to check if all the upgrades are bought, so we can display that inside the UI.
        foreach (Upgrade upgrade in playerUpgrades.Upgrades)
        {
            if (!upgrade.Unlocked)
            {
                allUpgradesUnlocked = false;
                playerUpgrades.NextUpgrade = upgrade;
            }
        }

        if (allUpgradesUnlocked)
        {
            // Disable buy button for this upgrade
        }
    }
}
