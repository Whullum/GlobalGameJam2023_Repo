using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public bool Unlocked { get; set; }

    [Tooltip("Value to add to the player stats.")]
    public float Value;
    [Tooltip("Amount of seeds needed to buy this upgrade.")]
    public int Cost;
}
