using UnityEngine;
using TMPro;

public class UI_UpgradesMenu : MonoBehaviour
{
    public UI_HubShopMenu hubShopMenuScript;
    private PlayerManager playerManager;

    [Header("Health")]
    public TextMeshProUGUI healthCurrentText;
    public TextMeshProUGUI healthUpgradeText;
    public TextMeshProUGUI healthCostText;

    [Header("Damage")]
    public TextMeshProUGUI damageCurrentText;
    public TextMeshProUGUI damageUpgradeText;
    public TextMeshProUGUI damageCostText;

    [Header("Move Speed")]
    public TextMeshProUGUI moveSpeedCurrentText;
    public TextMeshProUGUI moveSpeedUpgradeText;
    public TextMeshProUGUI moveSpeedCostText;


    // Start is called before the first frame update
    void Start()
    {
        // TODO: Initialize values
        // Display correct current and upgrade values
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void OnEnable()
    {
        hubShopMenuScript.ChangeShopTypeText("Upgrades");
    }

    public void UpdateValues(TextMeshProUGUI currentValueText, TextMeshProUGUI upgradeValueText, int currentValue, int upgradeValue)
    {
        currentValueText.text = currentValue.ToString();
        upgradeValueText.text = upgradeValue.ToString();
    }

    public void UpdateCost(TextMeshProUGUI text, int cost)
    {
        text.text = cost.ToString();
    }

    public void UpgradeHealth()
    {
        playerManager.Upgrade(UpgradeType.Health);
    }

    public void UpgradeDamage()
    {
        playerManager.Upgrade(UpgradeType.Attack);
    }

    public void UpgradeMoveSpeed()
    {
        playerManager.Upgrade(UpgradeType.Movement);
    }
}
