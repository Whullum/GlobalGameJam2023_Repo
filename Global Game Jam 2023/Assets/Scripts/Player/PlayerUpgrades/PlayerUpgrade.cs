using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Player Upgrade", menuName = "Player/Upgrade")]
public class PlayerUpgrade : ScriptableObject
{
    public string Name;
    [TextArea(5,20)]
    public string Description;
    public List<Upgrade> Upgrades = new List<Upgrade>();
}
