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

    // The multiplyer index of the player
    public int playerIndex;

    // The amount of money the player has
    [HideInInspector] public float cash;

    // A list of all the properties the player owns
    public List<Property> ownedProperties;

    // The UI element that displays the player's available cash
    [HideInInspector] public TextMeshProUGUI playerCashUI;

    // Start is called before the first frame update
    void Awake()
    {
        currentSpaceIndex = 0;

        // Set the player's starting cash
        cash = 200;

        // Create a new list of owned properties
        ownedProperties = new List<Property>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the player's cash UI
        playerCashUI.text = $"{cash} Dollars";
    }

    public void LandOnSpace()
    {
        currentSpace.OnLanded(this);
    }
}
