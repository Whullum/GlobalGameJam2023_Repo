using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
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
        seedsCounter.text = "SEEDS x" + seedsHeld;
    }

    public void ExitShop()
    {
        gameObject.SetActive(false);
    }
}
