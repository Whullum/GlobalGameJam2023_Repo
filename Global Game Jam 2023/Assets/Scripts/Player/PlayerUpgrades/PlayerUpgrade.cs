using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Upgrade", menuName = "Player/Upgrade")]
public class PlayerUpgrade : ScriptableObject
{
    [Tooltip("Name to show on the UI.")]
    public string Name;
    [Tooltip("Description to show on the UI.")]
    [TextArea(5,20)]
    public string Description;
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
