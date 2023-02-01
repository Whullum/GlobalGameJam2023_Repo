using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_UpgradesMenu : MonoBehaviour
{
    public UI_HubShopMenu hubShopMenuScript;

    [Header("Health")]
    public TextMeshProUGUI healthCurrentText;
    public TextMeshProUGUI healthUpgradeText;

    [Header("Damage")]
    public TextMeshProUGUI damageCurrentText;
    public TextMeshProUGUI damageUpgradeText;

    [Header("Move Speed")]
    public TextMeshProUGUI moveSpeedCurrentText;
    public TextMeshProUGUI moveSpeedUpgradeText;


    // Start is called before the first frame update
    void Start()
    {
        // TODO: Initialize values
        // Display correct current and upgrade values

        hubShopMenuScript.shopTypeText.text = "Upgrades";
    }

    public void UpdateValues(TextMeshProUGUI currentValueText, TextMeshProUGUI upgradeValueText, int currentValue, int upgradeValue)
    {
        currentValueText.text = currentValue.ToString();
        upgradeValueText.text = upgradeValue.ToString();
    }

    public void UpgradeHealth()
    {
        // TODO: Implement Player Health Upgrading
    }

    public void UpgradeDamage()
    {
        // TODO: Implement Player Damage Upgrading
    }

    public void UpgradeMoveSpeed()
    {
        // TODO: Implement Player Move Speed Upgrading
    }
}
