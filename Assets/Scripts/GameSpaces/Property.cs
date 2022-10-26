using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : PlayerSpace
{
    [Tooltip("How much this property costs to buy")]
    public float value;

    [Tooltip("This property's rent amount")]
    public float rent;

    [SerializeField, Tooltip("The Group This Property is a Part of")]
    string group;

    [HideInInspector, Tooltip("The amount of houses on this property")]
    public int houseCount;

    [HideInInspector, Tooltip("The amount of hotels on this property")]
    public int hotelCount;

    // The property's owner
    public Player owner = null;
        
    // Start is called before the first frame update
    void Start()
    {
        houseCount = 0;
        hotelCount = 0;
    }

    public override void OnLanded(Player _interactingPlayer)
    {

        base.OnLanded(_interactingPlayer);
        // Check if this property has an owner
        if (owner == null)
        {
            if (interactingPlayer.cash >= value)
            {
                // Set the ownership of this property to the interacting player
                owner = interactingPlayer;
                // Remove the cash from the player buying this property
                interactingPlayer.cash -= value;
                // Add this property to the player's list of properties
                owner.ownedProperties.Add(this);
            }
        }
        else
        {
            if(owner != interactingPlayer)
            {
                // Give the rent to the owning player from the interacting player
                interactingPlayer.cash -= rent;
                owner.cash += rent;
            }
        }
    }
}
