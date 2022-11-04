using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    // the current space the player is on
    [HideInInspector]
    public PlayerSpace currentSpace;

    // The array index of the current space
    [HideInInspector]
    public int currentSpaceIndex;

    // If the player is in jail or not
    [HideInInspector]
    public bool isInJail;

    // The multiplyer index of the player
    [HideInInspector]
    public int playerIndex;

    // The list of player tokens
    public GameObject[] tokens;

    // The token index the player chooses
    public GameObject currentToken;

    [Tooltip("The amount of cash the player begins the game with")]
    public float startingCash;

    // The amount of money the player has
    [HideInInspector] public float cash;

    // A list of all the properties the player owns
    public List<Property> ownedProperties;

    [HideInInspector, Tooltip("The amount of Get out of jail free cards this player has")]
    public int getOutOfJailCards;

    [HideInInspector, Tooltip("The amount of turns since the player went to jail")]
    public int turnsSinceJailed;

    [HideInInspector, Tooltip("Whether or not the player is moving")]
    public bool isMoving;

    // The UI element that displays the player's available cash
    [HideInInspector] public TextMeshProUGUI playerCashUI;

    // Start is called before the first frame update
    void Awake()
    {
        // The player starts on Go
        currentSpaceIndex = 0;

        // Set the player's starting cash
        cash = startingCash;

        // Create a new list of owned properties
        ownedProperties = new();

        // The player does not start in jail
        isInJail = false;

        // The player starts with no get out of jail free cards
        getOutOfJailCards = 0;

        // Turns since jailed is 0
        turnsSinceJailed = 0;

        //GetComponentInChildren<Animator>().StartPlayback();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCashUI != null)
        {
            // Update the player's cash UI
            playerCashUI.text = $"{cash} Dollars";
        }
    }

    public void LandOnSpace()
    {
        currentSpace.OnLanded(this);
    }

    public void SetPlayerToken(int tokenIndex)
    {
        currentToken = tokens[tokenIndex];
        for(int i = 0; i < tokens.Length; i++)
        {
            if(tokens[i] != currentToken)
            {
                Destroy(tokens[i]);
            }
        }
    }

    public void StartMoving()
    {
        StartCoroutine(MoveToken());
    }

    IEnumerator MoveToken()
    {
        // Time to move one space in s
        float moveDuration = 1.208f;

        float time = 0;
        while (time < moveDuration)
        {
            // lerp the player location and rotation to their next space
            transform.position = Vector3.Slerp(transform.position, currentSpace.transform.position, time / moveDuration);
            transform.rotation = Quaternion.Slerp(transform.rotation, currentSpace.transform.rotation, time / moveDuration);
            time += Time.deltaTime;
            yield return null;
        }
        isMoving = false;
        yield return null;
    }
}
