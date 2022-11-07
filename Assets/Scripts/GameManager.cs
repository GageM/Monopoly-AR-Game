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
    [HideInInspector]
    public List<Player> players;

    // the game board
    [HideInInspector]
    public GameBoard board;

    // The active player in the turn
    [HideInInspector]
    public int currentPlayerIndex;

    // Whether the AR system has been initialized
    bool initializedAR;

    // Whether the game has been started
    bool gameStarted;

    // The result of a player rolling dice
    private int rollResult;

    // The Text UI
    private TextMeshProUGUI UIText;

    // Debug Text on all screens
    [SerializeField] TextMeshProUGUI debugText;

    // The UI controller for the game
    public UIController uIController;

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
        initializedAR = false;

        // The game has not started

        // Get the UI Text to print game info
        UIText = GameObject.Find("UI_Text").GetComponent<TextMeshProUGUI>();

        // Set the raycastManager to its component on the object
        _raycastManager = GetComponent<ARRaycastManager>();

        // Create a list of all the players
        players = new List<Player>();  
    }

    // Update is called once per frame
    void Update()
    {
        // If the player does not touch the screen do nothing
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) { return; }

        if (initializedAR && !gameStarted)
        {
            // Perform AR raycast to any plane
            if (_raycastManager.Raycast(touch.position, Hits, TrackableType.Planes))
            {
                // If a plane was hit, create a game board at the location
                InitializeGame(Hits[0]);
            }
        }
    }

    public void RollDice()
    {
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

        // Move the player to their next space        
        StartCoroutine(MovePLayerByRoll(currentPlayerIndex));
    }

    public IEnumerator MovePLayerByRoll(int _playerIndex)
    {
        // Tell the player to move by the roll result
        players[_playerIndex].StartCoroutine(players[_playerIndex].MoveByRoll(rollResult));

        yield return null;
    }

    public IEnumerator MovePlayerToSpace(int _playerIndex, int _spaceIndex, bool cashOnPassGo)
    {
        // Move the player to the space
        players[_playerIndex].StartCoroutine(players[_playerIndex].MoveToSpace(_spaceIndex, cashOnPassGo));

        // End this player's turn
        if (_playerIndex == currentPlayerIndex)
        {
            EndTurn();
        }

        yield return null;
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
            players[i].SetPlayerToken(i);

            // Set the player's cash counter UI element
            players[i].playerCashUI = GameObject.Find($"Player {players[i].playerIndex + 1} Cash Counter").GetComponent<TextMeshProUGUI>();

            players[i].board = board;

            players[i].gameManager = this;

            // Initialize the player on the first space
            players[i].currentSpace = board.spaces[players[i].currentSpaceIndex];
            players[i].transform.SetPositionAndRotation(players[i].currentSpace.transform.position, players[i].currentSpace.transform.rotation);
        }

        // Tell the user that the game has been initialized
        StartCoroutine(UpdateUIText($"Game Initialized", 2f));

        gameStarted = true;

        currentPlayerIndex = 0;
        BeginTurn();
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

    public int EndTurn()
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

        uIController.BeginTurn(players[currentPlayerIndex]);

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
            uIController.BeginTurn(players[currentPlayerIndex]);
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
