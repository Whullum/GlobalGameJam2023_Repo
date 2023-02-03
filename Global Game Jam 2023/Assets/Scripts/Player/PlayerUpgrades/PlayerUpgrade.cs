using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Player Upgrade", menuName = "Player/Upgrade")]
public class PlayerUpgrade : ScriptableObject
{
    [Tooltip("Name to show on the UI.")]
    public string Name;
    [Tooltip("Description to show on the UI.")]
    [TextArea(5,20)]
    public string Description;
    [Tooltip("List containing all the upgrades the player can buy.")]
    public List<Upgrade> Upgrades = new List<Upgrade>();
    /// <summary>
    /// Current unlocked upgrade.
    /// </summary>
    public Upgrade CurrentUpgrade { get; set; }
    /// <summary>
    ///  Next upgrade to unlock.
    /// </summary>
    public Upgrade NextUpgrade { get; set; }
}
