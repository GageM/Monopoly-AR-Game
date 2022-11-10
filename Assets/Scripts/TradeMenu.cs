using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeMenu : MonoBehaviour
{
    // References to the two trading players
    public Player player1;
    public Player player2;

    // What Player 1 Offers in the trade
    List<Property> player1Properties;
    public float player1CashOffer;

    // What player 2 offers in the trade
    List<Property> player2Properties;
    public float player2CashOffer;

    // The toggle prefab for player property trading 
    [SerializeField] GameObject togglePrefab;

    // The parent scrollbox for each player's properties
    [SerializeField] Transform player1PropertiesContent;
    [SerializeField] Transform player2PropertiesContent;

    // A Reference to the UI Controller
    [SerializeField] UIController uIController;

    private void Awake()
    {
        // Create the lists for each playerr's trade options
        player1Properties = new List<Property>();
        player2Properties = new List<Property>();
    }

    public void OpenTradeMenu()
    {
        // Reset player cash offerings
        player1CashOffer = 0;
        player2CashOffer = 0;

        // Clear all the properties to trade
        player1Properties.Clear();
        player2Properties.Clear();

        foreach (RectTransform child in player1PropertiesContent)
        {
            Destroy(child);
        }

        foreach (RectTransform child in player2PropertiesContent)
        {
            Destroy(child);
        };


        // Fill the property trade options
        PopulateTradeOptions();
    }

    void PopulateTradeOptions()
    {
        // Get all the players' properties and add them to a list of toggles
        for(int i = 0; i < player1.ownedProperties.Count; i++)
        {
            AddNewCheckBox(player1.ownedProperties[i].name, player1.ownedProperties[i], player1PropertiesContent);
        }
        for (int i = 0; i < player2.ownedProperties.Count; i++)
        {
            AddNewCheckBox(player2.ownedProperties[i].name, player2.ownedProperties[i], player2PropertiesContent);
        }

    }

    GameObject AddNewCheckBox(string toggleText, Property property, Transform parent)
    {
        GameObject toggle = Instantiate(togglePrefab, parent);
        toggle.GetComponentInChildren<TextMeshProUGUI>().text = toggleText;
        toggle.GetComponent<PropertyToggle>().property = property;
        toggle.GetComponent<PropertyToggle>().menu = this;

        return toggle;
    }

    public void AddToPlayerProperties(Property property, Player player)
    {
        // add the propery to the list of properties to be traded
        if(player == player1)
        {
            player1Properties.Add(property);
        }
        else
        {
            player2Properties.Add(property);
        }
    }

    public void RemoveFromPlayerProperties(Property property, Player player)
    {
        // Remove the property from the list of properties to be traded
        if (player == player1)
        {
            player1Properties.Remove(property);
        }
        else
        {
            player2Properties.Remove(property);
        }
    }

    public void CancelTrade()
    {
        // Cancel the tade and reset the trade menu
        player1CashOffer = 0;
        player2CashOffer = 0;

        player1Properties.Clear();
        player2Properties.Clear();

        foreach (RectTransform child in player1PropertiesContent)
        {
            child.gameObject.SetActive(false);
            Destroy(child);
        }

        foreach (RectTransform child in player2PropertiesContent)
        {
            child.gameObject.SetActive(false);
            Destroy(child);
        }

        gameObject.SetActive(false);

        uIController.CloseTradeMenu();
        uIController.OpenPlayersToTradeWith();
    }

    public void CompleteTrade()
    {
        // Give the properties offered by player 1 to player 2
        for(int i = 0; i < player1Properties.Count; i++)
        {
            player1Properties[i].owner = player2;
            player2.ownedProperties.Add(player1Properties[i]);
            player1.ownedProperties.Remove(player1Properties[i]);
        }

        // Pay player 2 the money offered by player 1
        player1.cash -= player1CashOffer;
        player2.cash += player1CashOffer;

        // Give the properties offered by player 2 to player 1
        for (int j = 0; j < player2Properties.Count; j++)
        {
            player2Properties[j].owner = player1;
            player1.ownedProperties.Add(player2Properties[j]);
            player2.ownedProperties.Remove(player2Properties[j]);
        }

        // Pay player 1 the money offered by player 2
        player2.cash -= player2CashOffer;
        player1.cash += player2CashOffer;

        player1CashOffer = 0;
        player2CashOffer = 0;

        player1Properties.Clear();
        player2Properties.Clear();

        foreach(Transform child in player1PropertiesContent)
        {
            child.gameObject.SetActive(false);
            Destroy(child);
        }

        foreach (Transform child in player2PropertiesContent)
        {
            child.gameObject.SetActive(false);
            Destroy(child);
        }

        uIController.CloseTradeMenu();
        uIController.OpenTurnOptionsUI();
    }

    public void SetPlayer1Cash(float offer)
    {
        player1CashOffer = Mathf.Round(offer);
        uIController.player1CashAmount.text = $"$ {player1CashOffer}";
    }

    public void SetPlayer2Cash(float offer)
    {        
        player2CashOffer = Mathf.Round(offer);
        uIController.player2CashAmount.text = $"$ {player2CashOffer}";
    }
}
