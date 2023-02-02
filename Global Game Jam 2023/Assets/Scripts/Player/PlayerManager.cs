using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameObject player;
    private bool dashUnlocked = false;
    private bool reflectUnlocked = false;

    [Header("Player upgrades")]
    [SerializeField] private PlayerUpgrade attackUpgrade;
    [SerializeField] private PlayerUpgrade movementUpgrade;
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
        if(reflectUnlocked) return;

        player.AddComponent<ReflectAbility>();
    }

    public void UpgradeAttack()
    {
        foreach (Upgrade attack in attackUpgrade.Upgrades)
        {
            // If this upgrade is already unlocked, continue to the next one.
            if (attack.Unlocked) { continue; }

            // Add attack upgrade to player and set this upgrade to be unlocked.
            attack.Unlocked = true;
            break;
        }
        // We need to check if all the upgrades are bought, so we can display that inside the UI.
    }
}
