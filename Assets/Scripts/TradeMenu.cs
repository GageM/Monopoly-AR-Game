using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeMenu : MonoBehaviour
{
    public Player player1;
    public Player player2;

    List<Property> player1Properties;
    public float player1CashOffer;

    List<Property> player2Properties;
    public float player2CashOffer;

    [SerializeField] GameObject togglePrefab;

    [SerializeField] Transform player1PropertiesContent;
    [SerializeField] Transform player2PropertiesContent;

    [SerializeField] UIController uIController;

    private void Awake()
    {
        player1Properties = new List<Property>();
        player2Properties = new List<Property>();
    }

    public void OpenTradeMenu()
    {
        player1CashOffer = 0;
        player2CashOffer = 0;

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



        PopulateTradeOptions();
    }

    void PopulateTradeOptions()
    {
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
        player1CashOffer = 0;
        player2CashOffer = 0;

        player1Properties.Clear();
        player2Properties.Clear();

        foreach (RectTransform child in player1PropertiesContent)
        {
            Destroy(child);
        }

        foreach (RectTransform child in player2PropertiesContent)
        {
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

        foreach(RectTransform child in player1PropertiesContent)
        {
            Destroy(child);
        }

        foreach (RectTransform child in player2PropertiesContent)
        {
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
