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
    [SerializeField] TextMeshProUGUI buyHouseTitle;
    [SerializeField] TextMeshProUGUI sellHouseTitle;
    [SerializeField] TextMeshProUGUI buyHotelTitle;
    [SerializeField] TextMeshProUGUI sellHotelTitle;
    [SerializeField] TextMeshProUGUI mortgagePropertyText;

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

        playerCashPanel.gameObject.SetActive(true);
        topPanel.gameObject.SetActive(true);

        gameManager.StartAR();
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
        CloseJailOptionsUI();
        CloseTurnOptionsUI();
        ClosePlayerTurnUI();
        CloseEndTurnUI();
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
            jailOptions.gameObject.SetActive(true);
            jailOptions.enabled = true;

            jailOptions.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $" Player { currentPlayer.playerIndex + 1}: You're In Jail";
        }
    }

    public void CloseTurnOptionsUI()
    {
        turnOptions.gameObject.SetActive(false);
        turnOptions.enabled = false;
    }

    public void CloseJailOptionsUI()
    {
        jailOptions.gameObject.SetActive(false);
        jailOptions.enabled = false;
    }

    public void RollDice()
    {
        gameManager.RollDice();
        CloseTurnOptionsUI();
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
            AddNewButton(currentPlayer.ownedProperties[i].name, currentPlayer.ownedProperties[i]);
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

    public void EndTurn()
    {
        if(currentPlayer.isInJail)
        {
            currentPlayer.turnsSinceJailed++;
        }
        CloseEndTurnUI();

        gameManager.EndTurn();
    }
}
