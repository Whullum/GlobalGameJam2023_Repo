using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UI_PlayerDungeon : MonoBehaviour
{
    public static UI_PlayerDungeon Instance { get; private set; }
    public TextMeshProUGUI seedsCounterText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI abilityText;
    public TextMeshProUGUI abilityCountdownText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelTimerText;
    public Image equipedWeapon;
    public Slider healthSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    /// <summary>
    /// Change the display of the level time
    /// </summary>
    /// <param name="levelTime"> The time to set the text to </param>
    public void ChangeLevelTime(float levelTime)
    {
        var ts = TimeSpan.FromSeconds(levelTime);
        levelTimerText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }

    public void SetEquipedWeapon(Sprite weapon)
    {
        equipedWeapon.sprite = weapon;
    }

    public void ChangeWewaponText(string weapon) => weaponText.text = weapon;

    /// <summary>
    /// Set the text for the level number
    /// </summary>
    /// <param name="levelNumber"> The correct level number to display on the text </param>
    public void ChangeLevelText(int levelNumber)
    {
        levelText.text = levelNumber.ToString();
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
    public void ChangeAbilityCooldown(float cooldownTime)
    {
        var ts = TimeSpan.FromSeconds(cooldownTime);
        abilityCountdownText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }

    public void SetAbilityText(string text)
    {
        abilityCountdownText.text = text;
    }

    /// <summary>
    /// Changes the ability name according to the parameter
    /// </summary>
    /// <param name="abilityName">The chosen ability's name</param>
    public void ChangeAblityName(string abilityName)
    {
        abilityText.text = abilityName;
    }

    public void UpdateHealth(int minimum, int current, int maximum)
    {
        healthSlider.minValue = minimum;
        healthSlider.value = current;
        healthSlider.maxValue= maximum;
    }
}
