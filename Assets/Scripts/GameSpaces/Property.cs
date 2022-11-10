using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : PlayerSpace
{
    [Tooltip("How much this property costs to buy")]
    public float value;

    [Tooltip("This property's rent amount")]
    public float[] rent;

    [Tooltip("This property's sibling properties")]
    public List<Property> siblingProperties;

    [Tooltip("The group this property belongs to")]
    public int propertyGroup;

    [Tooltip("The cost of buying a house")]
    public float houseCost;

    [HideInInspector, Tooltip("The amount of houses on this property")]
    public int houseCount;

    [HideInInspector, Tooltip("The amount of hotels on this property")]
    public int hotelCount;

    [HideInInspector, Tooltip("Whether or not this property is mortgaged")]
    public bool isMortgaged;

    [Tooltip("The mortgage value of this property")]
    public float mortgageValue;

    [HideInInspector, Tooltip("Whether the owner can place a house on this property")]
    public bool canBuyHouse;

    [Tooltip("This Proerty's Colour")]
    public Color color;

    // The property's owner
    [HideInInspector]
    public Player owner = null;

    [SerializeField] GameObject house1;
    [SerializeField] GameObject house2;
    [SerializeField] GameObject house3;
    [SerializeField] GameObject house4;
    [SerializeField] GameObject hotel;
        
    // Start is called before the first frame update
    void Start()
    {
        houseCount = 0;
        hotelCount = 0;
        isMortgaged = false;

        GetHouseObjects();
    }

    public override void OnLanded(Player _interactingPlayer)
    {

        base.OnLanded(_interactingPlayer);
        // Check if this property has an owner
        if (owner == null)
        {
            if (interactingPlayer.cash >= value)
            {
                // Open the buy property option in the UI Controller
                interactingPlayer.gameManager.uIController.OpenBuyPropertyButton(value);

                // Open the end turn button
                interactingPlayer.gameManager.uIController.OpenEndTurnButton();
            }
            interactingPlayer.gameManager.uIController.OpenEndTurnButton();
        }
        else
        {
            if(owner != interactingPlayer && !isMortgaged)
            {
                if (interactingPlayer.cash >= ChooseRent())
                {
                    // Open the Pay Rent option in the UI Controller
                    interactingPlayer.gameManager.uIController.OpenPayRentButton(ChooseRent());
                    // Hide the End Turn option in the UI Controller
                    interactingPlayer.gameManager.uIController.CloseEndTurnButton();
                }
                else 
                {
                    interactingPlayer.gameManager.uIController.OpenGiveUpButton();
                }
            }
        }
    }

    public override void SpaceInteraction()
    {
        if (owner == null)
        {
                BuyProperty();
        }
        else
        {
            if (owner != interactingPlayer && !isMortgaged)
            {
                PayRent();
            }
        }
    }

    public virtual void PayRent()
    {
        if (interactingPlayer.cash > ChooseRent())
        {
            // Give the rent to the owning player from the interacting player
            interactingPlayer.cash -= ChooseRent();
            owner.cash += ChooseRent();

            interactingPlayer.gameManager.uIController.ClosePayRentButton();
            interactingPlayer.gameManager.uIController.OpenEndTurnButton();
        }
    }

    public virtual void BuyProperty()
    {
        // Set the ownership of this property to the interacting player
        owner = interactingPlayer;
        // Remove the cash from the player buying this property
        interactingPlayer.cash -= value;
        // Add this property to the player's list of properties
        interactingPlayer.ownedProperties.Add(this);
        // Sort the owning player's property list // This Function Doesn't work
        //interactingPlayer.SortOwnedProperties();

        interactingPlayer.gameManager.uIController.CloseBuyPropertyButton();
        interactingPlayer.gameManager.uIController.OpenEndTurnButton();
    }

    public virtual bool CheckIfCanBuyHouse()
    {
        for(int i = 0; i < siblingProperties.Count; i++)
        {
            if(siblingProperties[i].owner == owner)
            {
                canBuyHouse = true;
            }
            else
            {
                canBuyHouse = false;
                break;
            }
        }
        return canBuyHouse;
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
        owner.UpdatePlayerCashUI();

        UpdatePropertyMeshes();
    }

    public void AddHotel()
    {
        if (houseCount == 4 && owner.cash >= houseCost)
        {
            hotelCount++;
            owner.cash -= houseCost;
            houseCount = 0;
        }
        owner.UpdatePlayerCashUI();

        UpdatePropertyMeshes();
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
        owner.UpdatePlayerCashUI();

        UpdatePropertyMeshes();
    }

    public void SellHotel()
    {
        if (hotelCount > 0)
        {
            hotelCount--;
            owner.cash += (0.5f * houseCost);
        }
        owner.UpdatePlayerCashUI();

        UpdatePropertyMeshes();
    }

    public bool Mortgage()
    {
        if(!isMortgaged)
        {
            isMortgaged = true;
            owner.cash += mortgageValue;

            if(CheckIfCanBuyHouse())
            {
                for(int i = 0; i < siblingProperties.Count; i++)
                {
                    // Sell off all houses
                    if(siblingProperties[i].houseCount > 0)
                    {
                        owner.cash += 0.5f * siblingProperties[i].houseCount * siblingProperties[i].houseCost;
                        siblingProperties[i].houseCount = 0;    
                    }
                    // Sell off all hotels
                    if(siblingProperties[i].hotelCount > 0)
                    {
                        owner.cash += 0.5f * siblingProperties[i].hotelCount * siblingProperties[i].houseCost;
                        siblingProperties[i].hotelCount = 0;
                    }
                }
            }
        }
        else
        {
            if (owner.cash >= mortgageValue)
            {
                isMortgaged = false;
                owner.cash -= mortgageValue;
            }
        }

        owner.UpdatePlayerCashUI();
        return isMortgaged;
    }

    public virtual float ChooseRent()
    {
        if(hotelCount > 0)
        {
            return rent[5];
        }

        return houseCount switch
        {
            0 => rent[0],
            1 => rent[1],
            2 => rent[2],
            3 => rent[3],
            4 => rent[4],
            _ => rent[0],
        };
    }

    public int CompareTo(Property comparable)
    {
        var a = propertyGroup;
        var b = comparable.propertyGroup;

        if(a < b)
        {
            return -1;
        }
        if(a > b)
        {
            return 1;
        }
        return 0;
    }

    public virtual void UpdatePropertyMeshes()
    {
        switch (houseCount)
        {
            case 1:
                house1.SetActive(true);
                house2.SetActive(true);
                house3.SetActive(true);
                house4.SetActive(true);
                break;
            case 2:
                house1.SetActive(true);
                house2.SetActive(true);
                house1.SetActive(false);
                house2.SetActive(false);
                break;
            case 3:
                house1.SetActive(true);
                house2.SetActive(true);
                house3.SetActive(true);
                house3.SetActive(false);
                break;
            case 4:
                house1.SetActive(true);
                house2.SetActive(true);
                house3.SetActive(true);
                house4.SetActive(true);
                break;
            default:
                break;

        }
        if(hotelCount == 1)
        {
            hotel.SetActive(true);
        }
        else
        {
            hotel.SetActive(false);
        }
    }

    public virtual void GetHouseObjects()
    {

        house1.SetActive(false);
        house2.SetActive(false);
        house3.SetActive(false);
        house4.SetActive(false);
        hotel.SetActive(false);
    }
}
