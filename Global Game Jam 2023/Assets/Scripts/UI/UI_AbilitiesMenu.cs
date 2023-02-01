using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_AbilitiesMenu : MonoBehaviour
{
    public UI_HubShopMenu hubShopMenuScript;

    public TextMeshProUGUI dashCostText;
    public TextMeshProUGUI reflectCostText;

    private bool dashBought;
    private bool reflectBought;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Initialize shop values
        hubShopMenuScript.shopTypeText.text = "Abilities";
    }

    public void BuyDashAbility()
    {
        // if player has enough seeds, buy ability
        // dashBought = true;
    }

    public void BuyReflectAbility()
    {
        // if player has enough seeds, buy ability
        // reflectBought = true;
    }

    public void UpdateCostValues()
    {
        if (reflectBought)
            reflectCostText.text = "Purchased!";

        if (dashBought)
            dashCostText.text = "Purchased!";
    }
}
