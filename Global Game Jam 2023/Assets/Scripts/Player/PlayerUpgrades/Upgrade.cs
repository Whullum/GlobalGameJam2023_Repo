using UnityEngine;

[System.Serializable]
public class Upgrade
{
    

    [Tooltip("Value to add to the player stats.")]
    public int Value;
    [Tooltip("Amount of seeds needed to buy this upgrade.")]
    public int Cost;
    public bool Unlocked;
}
