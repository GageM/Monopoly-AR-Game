using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Cache ARRaycastManager GameObject from ARCoreSession
    private ARRaycastManager _raycastManager;

    // List for raycast hits is re-used by raycast manager
    private static readonly List<ARRaycastHit> Hits = new();

    [Tooltip("The player prefab")]
    public GameObject playerPrefab;

    [Tooltip("The game board prefab")]
    public GameObject boardPrefab;

    [Tooltip("The AR Session prefab")]
    public GameObject ARSessionPrefab;

    [Tooltip("The number of players")]
    public int numberOfPlayers;

    // An array of all the game's players
    public List<Player> players;

    // the game board
    [HideInInspector]
    public GameBoard board;

    // The active player in the turn
    int currentPlayerIndex;

    // Whether the AR system has been initialized
    bool initializedAR;

    // Whether the game has started or not
    bool gameStarted;

    // Whether the player can roll the dice
    bool canRoll;

    // The result of a player rolling dice
    private int rollResult;

    // The Text UI
    private TextMeshProUGUI UIText;

    // Debug Text on all screens
    [SerializeField] TextMeshProUGUI debugText;

    // The materials for each player
    [SerializeField, Tooltip("The material for each player")]
    Material[] playerMaterials;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // Has not yet initialized AR
        initializedAR = true;

        // Get the UI Text to print game info
        UIText = GameObject.Find("UI_Text").GetComponent<TextMeshProUGUI>();

        // Set the raycastManager to its component on the object
        _raycastManager = GetComponent<ARRaycastManager>();

        // Create a list of all the players
        players = new List<Player>();

        // Tell the manager that the game has not started yet
        gameStarted = false;

        // Tell the manager that no players can roll the dice yet
        canRoll = false;    
    }

    // Update is called once per frame
    void Update()
    {
        // If the player does not touch the screen do nothing
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) { return; }
        
        if (initializedAR)
        {            
            if (gameStarted)
            {
                // check if the player can start their turn
                if(canRoll)
                {
                    // If the player isn't in jail, continue turn normally
                    if (!players[currentPlayerIndex].isInJail)
                    {
                        BeginTurn();
                    }
                    // if the player is in jail
                    else
                    {
                        BeginTurnFromJail();
                    }
                }
            }

            // initialize the game
            else
            {
                // Perform AR raycast to any plane
                if (_raycastManager.Raycast(touch.position, Hits, TrackableType.Planes))
                {
                    // If a plane was hit, create a game board at the location
                    InitializeGame(Hits[0]);
                }
            }
        }
    }

    public int RollDice()
    {
        // Prevent the player from rolling again until the next turn
        canRoll = false;

        // create integers for the two dice results
        int die1Result;
        int die2Result;

        // Set the dice results to a random value between 1 & 6
        die1Result = Random.Range(1, 6);
        die2Result = Random.Range(1, 6);

        // Set rollResult to the result of the dice roll
        rollResult = die1Result + die2Result;

        // Print the roll result to the screen
        UIText.text = $"Rolled a {rollResult}";

        return rollResult;
    }

    public IEnumerator MovePLayerByRoll(int _playerIndex)
    {
        while (rollResult > 0)
        {
            // Set the UI text to the amount of spaces left to move
            UIText.text = $"{rollResult} Spaces left to move";

            // if the player is moving after being in jail start them from Just Visiting
            if (players[_playerIndex].currentSpaceIndex == 40)
            {
                players[_playerIndex].currentSpaceIndex = 10;
            }

            // If the player still has spaces before the end of the board
            if (players[_playerIndex].currentSpaceIndex < board.spaces.Length - 2)
            {
                // Move to the next space if there is a next space
                players[_playerIndex].currentSpaceIndex++;
                players[_playerIndex].currentSpace = board.spaces[players[_playerIndex].currentSpaceIndex];
                players[_playerIndex].transform.SetPositionAndRotation(players[_playerIndex].currentSpace.transform.position, players[_playerIndex].currentSpace.transform.rotation);
            }

            // If the player passes GO
            else
            {
                // Move to the first space if at the end of the board
                players[_playerIndex].currentSpaceIndex = 0;
                players[_playerIndex].currentSpace = board.spaces[0];
                players[_playerIndex].transform.SetPositionAndRotation(players[_playerIndex].currentSpace.transform.position, players[_playerIndex].currentSpace.transform.rotation);

                // Call the PassGO function since the player passed go
                PassGO(_playerIndex);
            }

            // decrement the roll result 
            rollResult--;

            // delay between moves
            yield return new WaitForSeconds(0.2f);
        }
        // Clear the UI text when finished moving
        UIText.text = string.Empty;

        LandOnSpace(_playerIndex);

        if (_playerIndex == currentPlayerIndex)
        {
            EndTurn();
        }
    }

    public IEnumerator MovePlayerToSpace(int _playerIndex, int _spaceIndex, bool cashOnPassGo)
    {
        while (players[_playerIndex].currentSpace != board.spaces[_spaceIndex])
        {
            // If the player still has spaces before the end of the board
            if (players[_playerIndex].currentSpaceIndex < board.spaces.Length - 2)
            {
                // Move to the next space if there is a next space
                players[_playerIndex].currentSpaceIndex++;
                players[_playerIndex].currentSpace = board.spaces[players[_playerIndex].currentSpaceIndex];
                players[_playerIndex].transform.SetPositionAndRotation(players[_playerIndex].currentSpace.transform.position, players[_playerIndex].currentSpace.transform.rotation);
            }

            // If the player passes GO
            else
            {
                // Move to the first space if at the end of the board
                players[_playerIndex].currentSpaceIndex = 0;
                players[_playerIndex].currentSpace = board.spaces[0];
                players[_playerIndex].transform.SetPositionAndRotation(players[_playerIndex].currentSpace.transform.position, players[_playerIndex].currentSpace.transform.rotation);

                if (cashOnPassGo)
                {
                    // Call the PassGO function since the player passed go
                    PassGO(_playerIndex);
                }
            }
        }
        // Clear the UI text when finished moving
        UIText.text = string.Empty;

        LandOnSpace(_playerIndex);

        if (_playerIndex == currentPlayerIndex)
        {
            EndTurn();
        }

        yield return "Player Moved";
    }

    public void PassGO(int _playerIndex)
    {
        // Give the player $200 for passing GO!
        players[_playerIndex].cash += 200;
        StartCoroutine(UpdateUIText($"You Passed GO!", 2));
    }

    void LandOnSpace(int _playerIndex)
    {
        // Set the UI text to display the space the player landed on
        StartCoroutine(UpdateUIText(players[_playerIndex].currentSpace.name, 3f));

        // Call the player's Land On Space Function
        players[currentPlayerIndex].LandOnSpace();
    }

    public void InitializeGame(ARRaycastHit hit)
    {
        // Create a new instance off the game board where the player taps
        board = Instantiate(boardPrefab, hit.pose.position, hit.pose.rotation).GetComponent<GameBoard>();
        board.gameObject.AddComponent<ARAnchor>();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            // Instantiate new player
            Player temp = Instantiate(playerPrefab, board.transform).GetComponent<Player>();

            // Set the player's player index
            temp.playerIndex = i;

            // Add to the list of players
            players.Add(temp);

            // Set the player's material
            players[i].gameObject.GetComponentInChildren<MeshRenderer>().material = playerMaterials[i];

            // Set the player's cash counter UI element
            players[i].playerCashUI = GameObject.Find($"Player {players[i].playerIndex + 1} Cash Counter").GetComponent<TextMeshProUGUI>();

            // Initialize the player on the first space
            players[i].currentSpace = board.spaces[players[i].currentSpaceIndex];
            players[i].transform.SetPositionAndRotation(players[i].currentSpace.transform.position, players[i].currentSpace.transform.rotation);
        }

        // Register that the game has started
        gameStarted = true;

        // Allow the first player to roll the dice
        canRoll = true;

        // Tell the user that the game has been initialized
        StartCoroutine(UpdateUIText($"Game Initialized", 2f));

    }

    public IEnumerator UpdateUIText(string text, float duration)
    {

        // Set the UI text to 'text' for 'duration' seconds
        UIText.text = text;

        yield return new WaitForSeconds(duration);

        if (UIText.text == text)
        {
            UIText.text = string.Empty;
        }
    }

    IEnumerator UpdateDebugText(string text, float duration)
    {

        // Set the UI text to 'text' for 'duration' seconds
        debugText.text = text;

        yield return new WaitForSeconds(duration);

        if (debugText.text == text)
        {
            debugText.text = string.Empty;
        }
    }

    int EndTurn()
    {
        // Change the active player to the next player
        if (currentPlayerIndex < numberOfPlayers - 1)
        {
            currentPlayerIndex++;
        }
        else
        {
            currentPlayerIndex = 0;
        }

        // Allow the next player to roll the dice
        canRoll = true;

        return currentPlayerIndex;
    }

    public void SetNumberOfPlayers(int _numberOfPlayers)
    {
        // Set the number of players to the selected amount
        numberOfPlayers = _numberOfPlayers;

        // Display the number of players on the Debug Text
        StartCoroutine(UpdateDebugText($"{ _numberOfPlayers} Players", 2));
    }

    public void StartAR()
    {
        // If numberOfPlayers is uninitialized, initialize it to 2
        if (numberOfPlayers < 2 || numberOfPlayers > 4)
        {
            numberOfPlayers = 2;
        }

        // Get the UI Text to print game info
        UIText = GameObject.Find("UI_Text").GetComponent<TextMeshProUGUI>();

        // Set the UI Text when this function is called
        StartCoroutine(UpdateUIText("Game Starting", 2));

        initializedAR = true;
    }

    void BeginTurn()
    {
        // Roll the dice
        RollDice();

        // Move the player to their next space        
        StartCoroutine(MovePLayerByRoll(currentPlayerIndex));
    }

    void BeginTurnFromJail()
    {
        StartCoroutine(UpdateUIText("Player is in jail", 2));

        // If the player has a get out of jail card
        if (players[currentPlayerIndex].getOutOfJailCards > 0)
        {
            // Reset turns since jailed
            players[currentPlayerIndex].turnsSinceJailed = 0;

            // The player is no longer in jail
            players[currentPlayerIndex].isInJail = false;

            // Subtract a get out of jail card
            players[currentPlayerIndex].getOutOfJailCards--;

            // If the player can roll the dice, then do that
            if (canRoll) RollDice();

            // Move the player to their next space   
            StartCoroutine(MovePLayerByRoll(currentPlayerIndex));
        }
        // if the player has no get out of jail cards
        else
        {
            // Free the player if they have been in jail for 3 or more turns
            if (players[currentPlayerIndex].turnsSinceJailed >= 3)
            {
                // Clear days since jailed
                players[currentPlayerIndex].turnsSinceJailed = 0;

                // The player is no longer in jail
                players[currentPlayerIndex].isInJail = false;

                // Pay jail fine
                players[currentPlayerIndex].cash -= 50;

                // Roll the dice
                RollDice();

                // Move the player to their next space        
                StartCoroutine(MovePLayerByRoll(currentPlayerIndex));
            }
            else
            {
                // Increse the amount of turns since the player was jailed
                players[currentPlayerIndex].turnsSinceJailed++;

                // End the player's turn
                EndTurn();
            }

        }
    }

    public void GoToJail(int _playerIndex)
    {
        // Tell the game the player is in jail
        players[_playerIndex].isInJail = true;

        // Set the player's space index to the jail space
        players[_playerIndex].currentSpaceIndex = 40;

        // Set the player's current space to the jail space
        players[_playerIndex].currentSpace = board.spaces[players[_playerIndex].currentSpaceIndex];

        // Move the player to jail
        players[_playerIndex].transform.SetPositionAndRotation(players[_playerIndex].currentSpace.transform.position, players[_playerIndex].currentSpace.transform.rotation);
    }
}
