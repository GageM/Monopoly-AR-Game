using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
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
    [SerializeField] Image mortgageMenu;


    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject buttonPrefab;

    Player currentPlayer;

    Property currentProperty;

    // Debug Text on all screens
    [SerializeField] TextMeshProUGUI debugText;


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

        CloseMortgageMenu();
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
    }

    public void ClosePropertyOptions()
    {
        propertyOptions.gameObject.SetActive(false);
        propertyOptions.enabled = false;
    }

    public void OpenHouseMenu()
    {
        houseMenu.gameObject.SetActive(true);
        houseMenu.enabled = true;
    }

    public void CloseHouseMenu()
    {
        houseMenu.gameObject.SetActive(false);
        houseMenu.enabled = false;
    }

    public void OpenHotelMenu()
    {
        hotelMenu.gameObject.SetActive(true);
        hotelMenu.enabled = true;
    }

    public void CloseHotelMenu()
    {
        hotelMenu.gameObject.SetActive(false);
        hotelMenu.enabled = false;
    }

    public void OpenMortgageMenu()
    {
        mortgageMenu.gameObject.SetActive(true);
        mortgageMenu.enabled = true;
    }

    public void CloseMortgageMenu()
    {
        mortgageMenu.gameObject.SetActive(false);
        mortgageMenu.enabled = false;
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
