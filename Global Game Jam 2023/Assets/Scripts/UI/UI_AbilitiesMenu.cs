using UnityEngine;
using TMPro;

public class UI_AbilitiesMenu : MonoBehaviour
{
    public UI_HubShopMenu hubShopMenuScript;
    private PlayerManager playerManager;

    public TextMeshProUGUI dashCostText;
    public TextMeshProUGUI reflectCostText;

    private bool dashBought;
    private bool reflectBought;

    // Start is called before the first frame update
    void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void OnEnable()
    {
        hubShopMenuScript.ChangeShopTypeText("Abilities");
        dashCostText.text = playerManager.DashCost;
        reflectCostText.text = playerManager.ReflectCost;
        dashBought = PlayerManager.DashUnlocked;
        reflectBought= PlayerManager.ReflectUnlocked;
        UpdateCostValues();
    }

    public void BuyDashAbility()
    {
        if (dashBought) return;

        dashBought = playerManager.UnlockDashAbility();

        if (dashBought)
            hubShopMenuScript.UpdateSeedsCounter(SeedWallet.Seeds);

        UpdateCostValues();
    }

    public void BuyReflectAbility()
    {
        if (reflectBought) return;

        reflectBought = playerManager.UnlockReflectAbility();
        
        if(reflectBought)
        hubShopMenuScript.UpdateSeedsCounter(SeedWallet.Seeds);

        UpdateCostValues();
    }

    private void UpdateCostValues()
    {
        if (reflectBought)
            reflectCostText.text = "Purchased!";

        if (dashBought)
            dashCostText.text = "Purchased!";
    }
}
