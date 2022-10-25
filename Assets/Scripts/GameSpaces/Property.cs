using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : PlayerSpace
{
    [SerializeField, Tooltip("How much this property costs to buy")]
    public float value;

    [SerializeField, Tooltip("This property's rent amount")]
    public float rent;

    [SerializeField, Tooltip("The Group This Property is a Part of")]
    string group;

    // The property's owner
    public Player owner = null;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnLanded(Player interactingPlayer)
    {
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
