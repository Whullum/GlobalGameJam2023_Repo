using UnityEngine;

[CreateAssetMenu(fileName = "New Player Upgrade", menuName = "Player/Upgrade")]
public class PlayerUpgrade : ScriptableObject
{
    [Tooltip("List containing all the upgrades the player can buy.")]
    public Upgrade[] Upgrades;
    /// <summary>
    /// Current unlocked upgrade.
    /// </summary>
    public Upgrade CurrentUpgrade { get; set; }
    /// <summary>
    ///  Next upgrade to unlock.
    /// </summary>
    public Upgrade NextUpgrade { get; set; }
    /// <summary>
    /// Tells if all the upgrades are unlocked.
    /// </summary>
    public bool AllUpgradesUnlocked;
}
