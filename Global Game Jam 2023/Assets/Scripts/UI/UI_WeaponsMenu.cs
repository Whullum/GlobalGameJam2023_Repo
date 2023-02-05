using TMPro;
using UnityEngine;

public class UI_WeaponsMenu : MonoBehaviour
{
    public UI_HubShopMenu hubShopMenuScript;
    private PlayerManager playerManager;

    public TextMeshProUGUI swordCostText;
    public TextMeshProUGUI bowCostText;

    private bool swordBought;
    private bool bowBought;

    // Start is called before the first frame update
    void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void OnEnable()
    {
        hubShopMenuScript.ChangeShopTypeText("Weapons");
        swordCostText.text = playerManager.SwordCost;
        bowCostText.text = playerManager.BowCost;
        swordBought = PlayerManager.SwordUnlocked;
        bowBought = PlayerManager.BowUnlocked;
        UpdateCostValues();
    }

    public void BuySword()
    {
        if (swordBought) return;

        swordBought = playerManager.UnlockSword();

        if (swordBought)
            hubShopMenuScript.UpdateSeedsCounter(SeedWallet.Seeds);

        UpdateCostValues();
    }

    public void BuyBow()
    {
        if (bowBought) return;

        bowBought = playerManager.UnlockBow();

        if (bowBought)
            hubShopMenuScript.UpdateSeedsCounter(SeedWallet.Seeds);

        UpdateCostValues();
    }

    private void UpdateCostValues()
    {
        if (bowBought)
            bowCostText.text = "Purchased!";

        if (swordBought)
            swordCostText.text = "Purchased!";
    }
}
