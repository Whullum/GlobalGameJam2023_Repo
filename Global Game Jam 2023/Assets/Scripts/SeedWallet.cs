using UnityEngine;

public class SeedWallet
{
    /// <summary>
    /// Total seeds the player has.
    /// </summary>
    public static int Seeds { get { return seeds; } }

    private static int seeds = 0;

    /// <summary>
    /// Adds seeds to the wallet.
    /// </summary>
    /// <param name="amount">Amount of seeds to add.</param>
    public static void CollectSeeds(int amount) => seeds += amount;

    /// <summary>
    /// Tries to spend an specified amount of seeds.
    /// </summary>
    /// <param name="amouunt">Amount of seeds to spend.</param>
    /// <returns>If the seeds were spend.</returns>
    public static bool SpendSeeds(int amouunt)
    {
        // Can't afford it
        if (seeds - amouunt < 0) return false;

        seeds -= amouunt;

        return true;
    }
}
