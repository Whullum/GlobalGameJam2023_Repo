using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Player/Weapon")]
public class Weapon : ScriptableObject
{
    public int Cost;
    public bool Unlocked;
}
