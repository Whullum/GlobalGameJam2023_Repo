using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerDungeon : MonoBehaviour
{
    public TextMeshProUGUI seedsCounterText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI abilityText;
    public TextMeshProUGUI abilityCountdownText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelTimerText;

    /// <summary>
    /// Change the display of the level time
    /// </summary>
    /// <param name="levelTime"> The time to set the text to </param>
    public void ChangeLevelTime(Time levelTime)
    {
        levelTimerText.text = levelTime.ToString();
    }

    /// <summary>
    /// Set the text for the level number
    /// </summary>
    /// <param name="levelNumber"> The correct level number to display on the text </param>
    public void ChangeLevelText(int levelNumber)
    {
        levelText.text = "LEVEL " + levelNumber;
    }

    /// <summary>
    /// Sets the text for the seed counter display
    /// </summary>
    /// <param name="seedCount">The number to set the seeds to</param>
    public void ChangeSeedsCount(int seedCount)
    {
        seedsCounterText.text = "SEEDS x" + seedCount;
    }

    /// <summary>
    /// Changes the time text display of the ability cooldown
    /// </summary>
    /// <param name="cooldownTime">The time to set the display</param>
    public void ChangeAbilityCooldown(Time cooldownTime)
    {
        abilityCountdownText.text = cooldownTime.ToString();
    }

    /// <summary>
    /// Changes the ability name according to the parameter
    /// </summary>
    /// <param name="abilityName">The chosen ability's name</param>
    public void ChangeAblityName(string abilityName)
    {
        abilityText.text = abilityName;
    }
}
