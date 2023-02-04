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

    [SerializeField] private GameObject upgradeNPC;
    [SerializeField] private GameObject weaponsNPC;
    [SerializeField] private GameObject abilityNPC;

    [SerializeField] private int upgradeNPCFloor = 2;
    [SerializeField] private int weaponNPCFloor = 4;
    [SerializeField] private int abilityNPCFloor = 6;

    [TextArea(5,20)]
    [SerializeField] private string upgradesNPCDialogue;
    [TextArea(5, 20)]
    [SerializeField] private string abilitiesNPCDialogue;
    [TextArea(5, 20)]
    [SerializeField] private string weaponsNPCDialogue;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();

        // TODO: Initialize shop values
        // Update seed counter to current amount of held seeds
        // Set dialogueNPC text
        LoadNPCButtons();
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
        ChangeNPCDialogue(abilitiesNPCDialogue);
    }

    public void ActivateUpgradesShop()
    {
        abilitiesShop.SetActive(false);
        upgradesShop.SetActive(true);
        weaponsShop.SetActive(false);
        playerManager.UpdateUpgradesUI();
        ChangeNPCDialogue(upgradesNPCDialogue);
    }

    public void ActivateWeaponsShop()
    {
        abilitiesShop.SetActive(false);
        upgradesShop.SetActive(false);
        weaponsShop.SetActive(true);
        ChangeNPCDialogue(weaponsNPCDialogue);
    }

    private void LoadNPCButtons()
    {
        if (DungeonManager.CurrentFloor >= upgradeNPCFloor)
            upgradeNPC.SetActive(true);
        else
            upgradeNPC.SetActive(false);

        /*if (DungeonManager.CurrentFloor >= weaponNPCFloor)
            weaponsNPC.SetActive(true);
        else
            weaponsNPC.SetActive(false);*/

        if (DungeonManager.CurrentFloor >= abilityNPCFloor)
            abilityNPC.SetActive(true);
        else
            abilityNPC.SetActive(false);
    }
}
