using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_HubShopMenu : MonoBehaviour
{
    public Button buyButton;
    public Button exitButton;

    public TextMeshProUGUI seedsCounter;
    public TextMeshProUGUI dialogueNPC;
    public TextMeshProUGUI shopTypeText;

    public GameObject abilitiesShop;
    public GameObject upgradesShop;
    public GameObject weaponsShop;
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();

        // TODO: Initialize shop values
        // Update seed counter to current amount of held seeds
        // Set dialogueNPC text
    }

    public void ChangeShopTypeText(string shopType)
    {
        shopTypeText.text = shopType;
    }

    public void ChangeNPCDialogue(string dialogue)
    {
        dialogueNPC.text = dialogue;
    }

    public void UpdateSeedsCounter(int seedsHeld)
    {
        seedsCounter.text = seedsHeld.ToString();
    }

    public void ExitShop()
    {
        gameObject.SetActive(false);
    }

    public void ActivateAbilitiesShop()
    {
        abilitiesShop.SetActive(true);
        upgradesShop.SetActive(false);
        weaponsShop.SetActive(false);
    }

    public void ActivateUpgradesShop()
    {
        abilitiesShop.SetActive(false);
        upgradesShop.SetActive(true);
        weaponsShop.SetActive(false);
        playerManager.UpdateUpgradesUI();
    }

    public void ActivateWeaponsShop()
    {
        abilitiesShop.SetActive(false);
        upgradesShop.SetActive(false);
        weaponsShop.SetActive(true); ;
    }
}
