using UnityEngine;

public class SeedGenerator
{
    /// <summary>
    /// Initializes Unity.Random class with the specified seed string.
    /// </summary>
    /// <param name="seed">Seed used to initialize Unity.Random class.</param>
    /// <returns>The generated seed.</returns>
    public static int GenerateSeed(string seed)
    {
        int generatedSeed;

        if (string.IsNullOrEmpty(seed))
            generatedSeed = (int)System.DateTime.Now.Ticks;
        else
            generatedSeed = seed.GetHashCode();

        Random.InitState(generatedSeed);

        return generatedSeed;
    }
}
