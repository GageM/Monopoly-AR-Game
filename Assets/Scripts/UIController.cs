using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("UI Panels")]
    // UI Panels
    [SerializeField] Image mainMenuUI;
    [SerializeField] Image playerCashPanel;
    [SerializeField] Image topPanel;
    [SerializeField] Image turnUI;
    [SerializeField] Image endTurnUI;
    [SerializeField] Image turnOptions;
    [SerializeField] Image jailOptions;
    [SerializeField] Image propertiesUI;
    [SerializeField] Transform propertiesList;
    [SerializeField] Image propertyOptions;
    [SerializeField] Image houseMenu;
    [SerializeField] Image hotelMenu;


    [Header("UI Elements")]
    // UI Elements
    [SerializeField] Button addHouseButton;
    [SerializeField] Button addHotelButton;
    [SerializeField] Button payJailFeeButton;
    [SerializeField] Button useGetOutOfJailButton;
    [SerializeField] TextMeshProUGUI buyHouseTitle;
    [SerializeField] TextMeshProUGUI sellHouseTitle;
    [SerializeField] TextMeshProUGUI buyHotelTitle;
    [SerializeField] TextMeshProUGUI sellHotelTitle;
    [SerializeField] TextMeshProUGUI mortgagePropertyText;
    [SerializeField] Image player1CashUI;
    [SerializeField] Image player2CashUI;
    [SerializeField] Image player3CashUI;
    [SerializeField] Image player4CashUI;
    [SerializeField] Button jailRollDiceButton;
    [SerializeField] Button jailEndTurnButton;

    [Header("End Turn UI Elements")]
    [SerializeField] Button endTurnButton;
    [SerializeField] Button payRentButton;
    [SerializeField] Button buyPropertyButton;
    [SerializeField] Button payTaxButton;
    [SerializeField] Button drawCardButton;
    [SerializeField] Button goToJailButton;
    [SerializeField] Button giveUpButton;
    [SerializeField] Button rollAgainButton;
    public TextMeshProUGUI endTurnTitle;


    [Header("Trade Menu UI")]
    [SerializeField] TradeMenu tradeMenu;
    [SerializeField] Image playersToTradeWith;
    [SerializeField] Button player1TradeButton;
    [SerializeField] Button player2TradeButton;
    [SerializeField] Button player3TradeButton;
    [SerializeField] Button player4TradeButton;
    public TextMeshProUGUI player1CashAmount;
    public TextMeshProUGUI player2CashAmount;
    [SerializeField] Slider player1CashSlider;
    [SerializeField] Slider player2CashSlider;

    [Header("End Game UI")]
    [SerializeField] Image endGameUI;
    [SerializeField] TextMeshProUGUI endGameTitle;
    
    [Space(10f)]
    // Debug Text on all screens
    [SerializeField] TextMeshProUGUI debugText;

    [Header("Game Manager & Prefabs")]
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject buttonPrefab;

    Player currentPlayer;
    Property currentProperty;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart());
    }

    public void SetNumberOfPlayers(int _numberOfPlayers)
    {
        gameManager.SetNumberOfPlayers(_numberOfPlayers);
    }

    public void StartGame()
    {
        mainMenuUI.gameObject.SetActive(false);
        mainMenuUI.enabled = false;

        OpenGameTextUI();

        gameManager.StartAR();
    }

    public void OpenGameTextUI()
    {
        topPanel.gameObject.SetActive(true);
        topPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Tap A Yellow Plane To Start The Game";
    }

    public void OpenPlayerCashUI()
    {
        playerCashPanel.gameObject.SetActive(true);

        if(gameManager.numberOfPlayers < 4)
        {
            player4CashUI.gameObject.SetActive(false);
        }
        
        if(gameManager.numberOfPlayers < 3)
        {
            player3CashUI.gameObject.SetActive(false);
        }
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        playerCashPanel.gameObject.SetActive(false);
        topPanel.gameObject.SetActive(false);

        CloseHotelMenu();
        CloseHouseMenu();
        ClosePropertyOptions();
        ClosePropertiesUI();
        CloseTurnOptionsUI();
        ClosePlayersToTradeWith();
        ClosePlayerTurnUI();
        CloseEndTurnUI();
        CloseTradeMenu();
        CloseEndGameUI();
    }

    public void BeginTurn(Player _player)
    {
        currentPlayer = _player;

        // Activate the turn UI
        turnUI.gameObject.SetActive(true);
        turnUI.enabled = true;

        OpenTurnOptionsUI();
    }

    public void ClosePlayerTurnUI()
    {
        turnUI.gameObject.SetActive(false);
        turnUI.enabled = false;
    }

    public void OpenTurnOptionsUI()
    {
        if (!currentPlayer.isInJail)
        {
            turnOptions.gameObject.SetActive(true);
            turnOptions.enabled = true;

            turnOptions.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $" Player { currentPlayer.playerIndex + 1}: It's Your Turn";
        }
        else
        {
            OpenJailOptionsUI();
        }
    }

    public void CloseTurnOptionsUI()
    {
        turnOptions.gameObject.SetActive(false);
        turnOptions.enabled = false;
        CloseJailOptionsUI();
    }

    public void OpenJailOptionsUI()
    {
        jailOptions.gameObject.SetActive(true);
        jailOptions.enabled = true;

        jailOptions.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $" Player { currentPlayer.playerIndex + 1}: You're In Jail";


        ClosePayJailFeeButton();
        CloseUseGetOutJailButton();
        if (currentPlayer != null)
        {
            if (currentPlayer.cash >= 200)
            {
                OpenPayJailFeeButton();
            }
        
            if (currentPlayer.getOutOfJailCards >= 1)
            {
                OpenUseGetOutJailButton();
            }
        }
    }

    public void CloseJailOptionsUI()
    {
        jailOptions.gameObject.SetActive(false);
        jailOptions.enabled = false;
    }

    public void OpenPayJailFeeButton()
    {
        payJailFeeButton.gameObject.SetActive(true);
        payJailFeeButton.enabled = true;
    }

    public void ClosePayJailFeeButton()
    {
        payJailFeeButton.gameObject.SetActive(false);
        payJailFeeButton.enabled = false;
    }

    public void OpenUseGetOutJailButton()
    {
        useGetOutOfJailButton.gameObject.SetActive(true);
        useGetOutOfJailButton.enabled = true;
    }

    public void CloseUseGetOutJailButton()
    {
        useGetOutOfJailButton.gameObject.SetActive(false);
        useGetOutOfJailButton.enabled = false;
    }

    public void RollDice()
    {
        if (currentPlayer.isInJail)
        {
            currentPlayer.RollDice();

            if(currentPlayer.rolledDoubles)
            {
                OpenTurnOptionsUI();
                currentPlayer.rolledDoubles = false;
                currentPlayer.doublesRollCount = 0;
            }
            else
            {
                jailRollDiceButton.gameObject.SetActive(false);
                jailEndTurnButton.gameObject.SetActive(true);
            }
        }
        else
        {
            currentPlayer.StartCoroutine(currentPlayer.MoveByRoll(currentPlayer.RollDice()));
            CloseTurnOptionsUI();
        }        
    }

    public void RollAgain()
    {
        rollAgainButton.gameObject.SetActive(false);

        currentPlayer.StartCoroutine(currentPlayer.MoveByRoll(currentPlayer.RollDice()));
        CloseTurnOptionsUI();
        CloseEndTurnUI();
    }

    public void PayToLeaveJail()
    {
        currentPlayer.PayToLeaveJail();
    }

    public void UseGetOutOfJailCard()
    {
        currentPlayer.UseGetOutOfJailCard();
    }

    public void OpenPropertiesUI()
    {
        propertiesUI.gameObject.SetActive(true);
        propertiesUI.enabled = true;

        // Populate the list of player properties
        for(int i = 0; i < gameManager.players[gameManager.currentPlayerIndex].ownedProperties.Count; i++)
        {
            GameObject button = AddNewButton(currentPlayer.ownedProperties[i].name, currentPlayer.ownedProperties[i]);
            button.GetComponent<Image>().color = currentPlayer.ownedProperties[i].color;
        }
    }

    public void ClosePropertiesUI()
    {
        for (int i = 0; i < propertiesList.childCount; i++)
        {
            Destroy(propertiesList.GetChild(i).gameObject);
        }

        propertiesUI.gameObject.SetActive(false);
        propertiesUI.enabled = false;
    }

    public void OpenPropertyOptions(Property property)
    {
        currentProperty = property;

        propertyOptions.gameObject.SetActive(true);
        propertyOptions.enabled = true;

        propertyOptions.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = property.name;

        // If the player can develop the property show the options
        if(currentProperty.CheckIfCanBuyHouse())
        {
            addHouseButton.gameObject.SetActive(true);
            addHouseButton.enabled = true;

            addHotelButton.gameObject.SetActive(true);
            addHotelButton.enabled = true;
        }

        else
        {
            addHouseButton.gameObject.SetActive(false);
            addHouseButton.enabled = false;

            addHotelButton.gameObject.SetActive(false);
            addHotelButton.enabled = false;
        }

        if (currentProperty.isMortgaged)
        {
            mortgagePropertyText.text = $"End Mortgage";
        }

        else
        {
            mortgagePropertyText.text = $"Mortgage";
        }

    }

    public void ClosePropertyOptions()
    {
        propertyOptions.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Empty;

        propertyOptions.gameObject.SetActive(false);
        propertyOptions.enabled = false;
    }

    public void OpenHouseMenu()
    {
        houseMenu.gameObject.SetActive(true);
        houseMenu.enabled = true;

        // Set house valuse in UI
        houseMenu.transform.Find("BuyHouseTitle").GetComponent<TextMeshProUGUI>().text = $"Buy Houses: -${currentProperty.houseCost}";
        houseMenu.transform.Find("SellHouseTitle").GetComponent<TextMeshProUGUI>().text = $"Buy Houses: +${0.5f * currentProperty.houseCost}";
    }

    public void CloseHouseMenu()
    {
        //Clear UI house values
        buyHouseTitle.text = string.Empty;
        sellHouseTitle.text = string.Empty;

        houseMenu.gameObject.SetActive(false);
        houseMenu.enabled = false;
    }

    public void OpenHotelMenu()
    {
        hotelMenu.gameObject.SetActive(true);
        hotelMenu.enabled = true;

        // Set UI hotel values
        hotelMenu.transform.Find("BuyHotelTitle").GetComponent<TextMeshProUGUI>().text = $"Buy Hotel: -${currentProperty.houseCost}";
        hotelMenu.transform.Find("SellHotelTitle").GetComponent<TextMeshProUGUI>().text = $"Buy Hotel: +${0.5f * currentProperty.houseCost}";
    }

    public void CloseHotelMenu()
    {
        // Clear UI Hotel Values
        buyHotelTitle.text = string.Empty;
        sellHotelTitle.text = string.Empty;

        hotelMenu.gameObject.SetActive(false);
        hotelMenu.enabled = false;
    }

    public void OpenEndTurnUI()
    {
        endTurnUI.gameObject.SetActive(true);
        endTurnUI.enabled = true;

        endTurnUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = currentPlayer.currentSpace.name;
    }

    public void CloseEndTurnUI()
    {
        CloseBuyPropertyButton();
        ClosePayRentButton();
        CloseDrawCardButton();
        CloseGiveUpButton();
        CloseGoToJailButton();
        CloseEndTurnButton();
        goToJailButton.gameObject.SetActive(false);
        rollAgainButton.gameObject.SetActive(false);

        endTurnUI.gameObject.SetActive(false);
        endTurnUI.enabled = false;
    }

    GameObject AddNewButton(string buttonText, Property property)
    {
        GameObject button;
        button = Instantiate(buttonPrefab, propertiesList);
        button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        button.GetComponent<Button>().onClick.AddListener(() => OpenPropertyOptions(property));
        button.GetComponent<Button>().onClick.AddListener(() => ClosePropertiesUI());

        return button;
    }

    public void AddHouse(int housesToAdd)
    {
        currentProperty.AddHouse(housesToAdd);
    }

    public void SellHouse(int housesToSell)
    {
        currentProperty.SellHouse(housesToSell);
    }

    public void AddHotel()
    {
        currentProperty.AddHotel();
    }

    public void SellHotel()
    {
        currentProperty.SellHotel();
    }

    public void Mortgage()
    {
        currentProperty.Mortgage();
        if(currentProperty.isMortgaged)
        {
            mortgagePropertyText.text = $"End Mortgage";
        }
        else
        {
            mortgagePropertyText.text = $"Mortgage";
        }
    }

    public void LandOnSpace()
    {
        currentPlayer.LandOnSpace();
    }

    public void EndTurn()
    {
        if(currentPlayer.isInJail)
        {
            currentPlayer.turnsSinceJailed++;
        }
        CloseEndTurnUI();
        CloseJailOptionsUI();

        gameManager.EndTurn();
    }

    public void OpenEndTurnButton()
    {
        if (currentPlayer.rolledDoubles && currentPlayer.doublesRollCount < 3)
        {
            rollAgainButton.gameObject.SetActive(true);
            currentPlayer.rolledDoubles = false;
            endTurnButton.gameObject.SetActive(false);
            endTurnButton.enabled = false;
        }
        else if (currentPlayer.rolledDoubles && currentPlayer.doublesRollCount >= 3)
        {
            goToJailButton.gameObject.SetActive(true);
            endTurnButton.gameObject.SetActive(false);
            goToJailButton.enabled = true;
            currentPlayer.rolledDoubles = false;
        }
        else
        {
            endTurnButton.gameObject.SetActive(true);
            endTurnButton.enabled = true;
            currentPlayer.rolledDoubles = false;
        }
    }

    public void CloseEndTurnButton()
    {
        endTurnButton.gameObject.SetActive(false);
        endTurnButton.enabled = false;
        rollAgainButton.gameObject.SetActive(false);
        goToJailButton.gameObject.SetActive(false);

    }

    public void SpaceInteraction()
    {
        currentPlayer.currentSpace.SpaceInteraction();
    }

    public void OpenPayRentButton(float rent)
    {
        payRentButton.gameObject.SetActive(true);
        payRentButton.enabled = true;

        payRentButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Pay Rent: ${rent}";
    }

    public void ClosePayRentButton()
    {
        payRentButton.gameObject.SetActive(false);
        payRentButton.enabled = false;
    }

    public void OpenBuyPropertyButton(float value)
    {
        buyPropertyButton.gameObject.SetActive(true);
        buyPropertyButton.enabled = true;

        buyPropertyButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Buy Property: ${value}";
    }

    public void CloseBuyPropertyButton()
    {
        buyPropertyButton.gameObject.SetActive(false);
        buyPropertyButton.enabled = false;
    }

    public void OpenPayTaxButton(float value)
    {
        payTaxButton.gameObject.SetActive(true);
        payTaxButton.enabled = true;

        payTaxButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Pay Tax: ${value}";
    }

    public void ClosePayTaxButton()
    {
        payTaxButton.gameObject.SetActive(false);
        payTaxButton.enabled = false;
    }

    public void OpenDrawCardButton()
    {
        drawCardButton.gameObject.SetActive(true);
        drawCardButton.enabled = true;
    }

    public void CloseDrawCardButton()
    {
        drawCardButton.gameObject.SetActive(false);
        drawCardButton.enabled = false;
    }

    public void GoToJail()
    {
        currentPlayer.GoToJail();

        currentPlayer.gameManager.uIController.CloseGoToJailButton();
        currentPlayer.gameManager.uIController.OpenEndTurnButton();
    }

    public void OpenGoToJailButton()
    {
        goToJailButton.gameObject.SetActive(true);
        goToJailButton.enabled = true;
    }

    public void CloseGoToJailButton()
    {
        goToJailButton.gameObject.SetActive(false);
        goToJailButton.enabled = false;
    }

    public void GiveUp()
    {
        gameManager.RemovePlayer(currentPlayer);
    }

    public void OpenGiveUpButton()
    {
        giveUpButton.gameObject.SetActive(true);
        giveUpButton.enabled = true;

        CloseEndTurnButton();
    }

    public void CloseGiveUpButton()
    {
        giveUpButton.gameObject.SetActive(false);
        giveUpButton.enabled = false;
    }

    public void OpenPlayersToTradeWith()
    {
        playersToTradeWith.gameObject.SetActive(true);

        if(currentPlayer.playerIndex != 0)
        player1TradeButton.gameObject.SetActive(true);

        if(currentPlayer.playerIndex != 1)
        player2TradeButton.gameObject.SetActive(true);

        if(currentPlayer.playerIndex != 2 && gameManager.players.Count >= 3)
        player3TradeButton.gameObject.SetActive(true);

        if(currentPlayer.playerIndex != 3 && gameManager.players.Count >= 4)
        player4TradeButton.gameObject.SetActive(true);
    }

    public void ClosePlayersToTradeWith()
    {
        player1TradeButton.gameObject.SetActive(false);
        player2TradeButton.gameObject.SetActive(false);
        player3TradeButton.gameObject.SetActive(false);
        player4TradeButton.gameObject.SetActive(false);

        playersToTradeWith.gameObject.SetActive(false);
    }

    public void OpenTradeMenu(int otherPlayer)
    {
        tradeMenu.gameObject.SetActive(true);
        tradeMenu.enabled = true;

        tradeMenu.GetComponent<TradeMenu>().player1 = currentPlayer;
        tradeMenu.GetComponent<TradeMenu>().player2 = gameManager.players[otherPlayer];

        player1CashSlider.maxValue = currentPlayer.cash;
        player1CashSlider.minValue = 0;

        player2CashSlider.maxValue = gameManager.players[otherPlayer].cash;
        player2CashSlider.minValue = 0;

        tradeMenu.OpenTradeMenu();
    }

    public void CloseTradeMenu()
    {
        tradeMenu.gameObject.SetActive(false);
        tradeMenu.enabled = false;
    }

    public void OpenEndGameUI()
    {
        ClosePlayerTurnUI();
        CloseEndTurnUI();

        endGameUI.gameObject.SetActive(true);
        endGameUI.enabled = true;
        endGameTitle.gameObject.SetActive(true);
        endGameTitle.enabled = true;

        endGameTitle.text = $" Player {currentPlayer.playerIndex + 1} Wins!";
    }

    public void CloseEndGameUI()
    {
        endGameTitle.gameObject.SetActive(false);
        endGameTitle.enabled = false;
        endGameUI.gameObject.SetActive(false);
        endGameUI.enabled = false;
    }
}
