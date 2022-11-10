using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Tooltip("Whether the game is being run on a mobile device")]
    public bool UsingAR;
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

    // The Text UI
    [HideInInspector] public TextMeshProUGUI UIText;

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

            // Get the UI Text to print game info
            UIText = GameObject.Find("UI_Text").GetComponent<TextMeshProUGUI>();

            // Set the raycastManager to its component on the object
            _raycastManager = GetComponent<ARRaycastManager>();

            // Create a list of all the players
            players = new List<Player>();

        if(!UsingAR)
        {
            InitializeGame(new ARRaycastHit());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the player does not touch the screen do nothing
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) { return; }

        // If the game isn't initialized, initialize it
        if (initializedAR && !gameStarted)
        {
            // Perform AR raycast to any plane
            if (_raycastManager.Raycast(touch.position, Hits, TrackableType.Planes))
            {
                // If a plane was hit, create a game board at the location
                InitializeGame(Hits[0]);
            }
        }
        else { return; }
    }

    public void InitializeGame(ARRaycastHit hit)
    {
        uIController.OpenPlayerCashUI();

        // Create a new instance off the game board where the player taps
        board = Instantiate(boardPrefab, hit.pose.position, hit.pose.rotation).GetComponent<GameBoard>();
        board.gameObject.AddComponent<ARAnchor>();

        //Initialize the players
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

        // The game has started now
        gameStarted = true;

        // Player 1 goes first
        currentPlayerIndex = 0;

        // Stop generating AR Trackers
        EndTrackingGeneration();

        // Begin playe 1's turn
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
        // Clear anything related to the player rolling doubles
        players[currentPlayerIndex].rolledDoubles = false;
        players[currentPlayerIndex].doublesRollCount = 0;

        // Change the active player to the next player
        if (currentPlayerIndex < numberOfPlayers - 1)
        {
            currentPlayerIndex++;
        }
        else
        {
            currentPlayerIndex = 0;
        }

        // Begin the next player's turn
        uIController.BeginTurn(players[currentPlayerIndex]);

        // Return the index of the next player
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

    public void BeginTurn()
    {
        // Tell the UI controller to show the player's turn UI
        uIController.BeginTurn(players[currentPlayerIndex]);
    }

    void EndTrackingGeneration()
    {
        // Stop Generating Planes!!!
        foreach(var plane in GetComponent<ARPlaneManager>().trackables)
        {
            plane.gameObject.SetActive(false);
        }
        GetComponent<ARPlaneManager>().enabled = false;

        // Stop Creating Point Clouds too!
        foreach(var pointCloud in GetComponent<ARPointCloudManager>().trackables)
        {
            pointCloud.gameObject.SetActive(false);
        }
        GetComponent<ARPointCloudManager>().enabled = false;
    }

    public void RemovePlayer(Player playerToRemove)
    {
        if (playerToRemove != null)
        {
            // Loop around to the next player
            currentPlayerIndex = 0; // currentPlayerIndex < (players.Count - 1) ? currentPlayerIndex + 1: 0;

            // Destroy the player's cash counter UI element
            Destroy(GameObject.Find($"Player {playerToRemove.playerIndex + 1} Cash Counter"));

            players.Remove(playerToRemove);
            Destroy(playerToRemove.gameObject);

            if (players.Count == 1)
            {
                BeginTurn();
                uIController.OpenEndGameUI();

            }
            else
            {
                uIController.CloseEndTurnUI();
                BeginTurn();
            }
        }
    }
}
