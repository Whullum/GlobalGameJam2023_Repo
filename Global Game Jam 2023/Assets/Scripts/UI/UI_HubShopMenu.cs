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

    public GameObject abilitiesImage;
    public GameObject upgradesImage;
    public GameObject weaponsImage;

    private PlayerManager playerManager;

    [SerializeField] private GameObject upgradeNPC;
    [SerializeField] private GameObject weaponsNPC;
    [SerializeField] private GameObject abilityNPC;

    [SerializeField] private int upgradeNPCFloor = 2;
    [SerializeField] private int weaponNPCFloor = 6;
    [SerializeField] private int abilityNPCFloor = 4;

    [TextArea(5,20)]
    [SerializeField] private string upgradesNPCDialogue;
    [TextArea(5, 20)]
    [SerializeField] private string abilitiesNPCDialogue;
    [TextArea(5, 20)]
    [SerializeField] private string weaponsNPCDialogue;

    public UI_MainMenuBehavior menu;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();

        // TODO: Initialize shop values
        // Update seed counter to current amount of held seeds
        // Set dialogueNPC text
        LoadNPCButtons();
        UpdateSeedsCounter(SeedWallet.Seeds);
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
        abilitiesImage.SetActive(true);

        upgradesShop.SetActive(false);
        upgradesImage.SetActive(false);

        weaponsShop.SetActive(false);
        weaponsImage.SetActive(false);

        ChangeNPCDialogue(abilitiesNPCDialogue);
        UpdateSeedsCounter(SeedWallet.Seeds);
    }

    public void ActivateUpgradesShop()
    {
        abilitiesShop.SetActive(false);
        abilitiesImage.SetActive(false);

        upgradesShop.SetActive(true);
        upgradesImage.SetActive(true);

        weaponsShop.SetActive(false);
        weaponsImage.SetActive(false);

        playerManager.UpdateUpgradesUI();
        ChangeNPCDialogue(upgradesNPCDialogue);
        UpdateSeedsCounter(SeedWallet.Seeds);
    }

    public void ActivateWeaponsShop()
    {
        abilitiesShop.SetActive(false);
        abilitiesImage.SetActive(false);

        upgradesShop.SetActive(false);
        upgradesImage.SetActive(false);

        weaponsShop.SetActive(true);
        weaponsImage.SetActive(true);

        ChangeNPCDialogue(weaponsNPCDialogue);
        UpdateSeedsCounter(SeedWallet.Seeds);
    }

    private void LoadNPCButtons()
    {
        if (DungeonManager.CurrentFloor >= upgradeNPCFloor)
        {
            upgradeNPC.SetActive(true);
            menu.stateHealth.SetValue();
        }
        else
            upgradeNPC.SetActive(false);

        if (DungeonManager.CurrentFloor >= abilityNPCFloor)
        {
            menu.stateHealthAbilities.SetValue();
            abilityNPC.SetActive(true);
        }
        else
            abilityNPC.SetActive(false);

        if (DungeonManager.CurrentFloor >= weaponNPCFloor)
        {
            weaponsNPC.SetActive(true);
            menu.stateAllNPC.SetValue();
        }
        else
            weaponsNPC.SetActive(false);
    }
}
