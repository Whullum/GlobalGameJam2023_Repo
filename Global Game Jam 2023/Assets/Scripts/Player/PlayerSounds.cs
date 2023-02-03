using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("Player Combat Sounds")]
    public AK.Wwise.Event playerAttackSound;
    public AK.Wwise.Event playerDamagedSound;

    [Header("Player Ability Sounds")]
    public AK.Wwise.Event dodgeAbilitySound;
    public AK.Wwise.Event reflectAbilitySound;
}
