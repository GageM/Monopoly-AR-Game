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

    [SerializeField, Tooltip("The cost of buying a house")]
    float houseCost;

    [HideInInspector, Tooltip("The amount of houses on this property")]
    public int houseCount;

    [HideInInspector, Tooltip("The amount of hotels on this property")]
    public int hotelCount;

    [HideInInspector, Tooltip("Whether or not this property is mortgaged")]
    public bool isMortgaged;

    // The property's owner
    public Player owner = null;
        
    // Start is called before the first frame update
    void Start()
    {
        houseCount = 0;
        hotelCount = 0;
        isMortgaged = false;
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
            if(owner != interactingPlayer && !isMortgaged)
            {
                // Give the rent to the owning player from the interacting player
                interactingPlayer.cash -= rent;
                owner.cash += rent;
            }
        }
    }

    public void AddHouse(int housesToAdd)
    {
        for (int i = 0; i < housesToAdd; i++)
        {
            if (houseCount < 4 && hotelCount == 0 && owner.cash >= houseCost)
            {
                houseCount++;
                owner.cash -= houseCost;
            }
            else break;
        }
    }

    public void AddHotel()
    {
        if (houseCount == 4 && owner.cash >= houseCost)
        {
            hotelCount++;
            owner.cash -= houseCost;
            houseCount = 0;
        }
    }

    public void SellHouse(int housesToSell)
    {
        for(int i = 0; i < housesToSell; i++)
        {
            if(houseCount > 0)
            {
                houseCount--;
                owner.cash += (0.5f * houseCost);
            }
        }
    }

    public void SellHotel()
    {
        if (hotelCount > 0)
        {
            hotelCount--;
            owner.cash += (0.5f * houseCost);
        }
    }

    public void Mortgage()
    {
        if(!isMortgaged)
        {
            isMortgaged = true;
            owner.cash += value;
        }
        else
        {
            isMortgaged = false;
            owner.cash -= value;
        }
    }
}
