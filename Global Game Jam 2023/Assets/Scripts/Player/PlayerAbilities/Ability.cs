using UnityEngine;

[CreateAssetMenu(fileName = "New Player Ability", menuName ="Player/Ability")]
public class Ability : ScriptableObject
{
    [TextArea(5,20)]
    public string Description;
    public int Cost;
    public bool Unlocked;
}
