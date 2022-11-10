using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chance : PlayerSpace
{
    GameManager manager;

    delegate void ChanceCards();

    readonly List<ChanceCards> chanceCards = new();

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("AR Session Origin").GetComponent<GameManager>();

        // Add the chance cards to the list
        chanceCards.Add(AdvanceToGo);
        chanceCards.Add(AdvanceToTrafalgarSquare);
        chanceCards.Add(AdvanceToPallMall);
        //chanceCards.Add(AdvanceToNearestRailroad);
        //chanceCards.Add(AdvanceToUtility);
        chanceCards.Add(BankDividend);
        chanceCards.Add(GetOutOfJailFree);
        //chanceCards.Add(GoBackSpaces);
        chanceCards.Add(GoToJail);
        //chanceCards.Add(PropertyRepair);
        chanceCards.Add(AdvanceToKingsCross);
        chanceCards.Add(AdvanceToMayfair);
        chanceCards.Add(PayPlayers);
        chanceCards.Add(LoanMatures);
    }

    public override void OnLanded(Player _interactingPlayer)
    {
        base.OnLanded(_interactingPlayer);

        interactingPlayer.gameManager.uIController.OpenDrawCardButton();
        interactingPlayer.gameManager.uIController.CloseEndTurnButton();
    }

    public override void SpaceInteraction()
    {
        // Choose a random card from the pile
        int topCard = Random.Range(0, chanceCards.Count - 1);

        // Call that card's function
        chanceCards[topCard]();

        interactingPlayer.gameManager.uIController.CloseDrawCardButton();

        interactingPlayer.gameManager.uIController.OpenEndTurnButton();
    }

    void AdvanceToGo()
    {
        manager.StartCoroutine(manager.UpdateUIText("Advance to Go, Collect $200", 2));
        // Advance to Go, Collect 200 Dollars
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(0, true));
    }

    void AdvanceToTrafalgarSquare()
    {
        manager.StartCoroutine(manager.UpdateUIText("Advance to Trafalgar Square. If you pass Go, collect $200", 2));
        // Advance to Trafalgar Square. If you pass Go, collect 200 Dollars
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(24, true));
    }

    void AdvanceToPallMall()
    {
        manager.StartCoroutine(manager.UpdateUIText("Advance to Pall Mall If you pass Go, collect $200.", 2));
        // Advance to Pall Mall If you pass Go, collect 200 Dollars.
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(11, true));
    }

    void AdvanceToUtility()
    {
        manager.StartCoroutine(manager.UpdateUIText("Advance token to the nearest Utility.", 2));
        // Advance token to the nearest Utility.

        // Determine the Utility to move to
        if (interactingPlayer.currentSpaceIndex >= 28 || interactingPlayer.currentSpaceIndex <= 12)
        {
            interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(12, true));
        }
        else
        {
            interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(28, true));
        }
    }

    void AdvanceToNearestRailroad()
    {
        manager.StartCoroutine(manager.UpdateUIText("Advance to the nearest Railroad.", 2));
        // Advance to the nearest Railroad.

        // Determine the Railway to move to
        if (interactingPlayer.currentSpaceIndex >= 36 || interactingPlayer.currentSpaceIndex <= 5)
        {
            interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(5, true));
        }
        else if (interactingPlayer.currentSpaceIndex > 5 && interactingPlayer.currentSpaceIndex <= 15)
        {
            interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(15, true));
        }
        else if (interactingPlayer.currentSpaceIndex > 15 && interactingPlayer.currentSpaceIndex <= 25)
        {
            interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(25, true));
        }
        else
        {
            interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(35, true));
        }
    }

    void BankDividend()
    {
        manager.StartCoroutine(manager.UpdateUIText("Bank pays you dividend of $50", 2));
        // Bank pays you dividend of 50 Dollars
        interactingPlayer.cash += 50;
    }

    void GetOutOfJailFree()
    {
        manager.StartCoroutine(manager.UpdateUIText("Get out of Jail Free", 2));

        // add a get out of jail free card to the player
        interactingPlayer.getOutOfJailCards++;
    }

    void GoBackSpaces()
    {
        manager.StartCoroutine(manager.UpdateUIText("Go back 3 spaces", 2));
        // Go back 3 spaces

        // Set the player's new space to 3 spaces behind their current space
        // TODO: Fix Go Back 3 Spaces Function
        int newSpaceIndex = (interactingPlayer.currentSpaceIndex <= 3) ? (interactingPlayer.currentSpaceIndex - 3) : (interactingPlayer.currentSpaceIndex + 39 - 3);

        // Move the player to their new space
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(newSpaceIndex, false));
    }

    void GoToJail()
    {
        manager.StartCoroutine(manager.UpdateUIText("Go directly to Jail. Do not pass Go, do not collect $200", 2));
        // Go directly to Jail. Do not pass Go, do not collect 200 Dollars

        interactingPlayer.isInJail = true;
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(40, true));
    }

    void PropertyRepair()
    {
        manager.StartCoroutine(manager.UpdateUIText("Make general repairs on all your properties: For each house pay 25 Dollars, For each hotel pay $100", 2));
        // Make general repairs on all your properties: For each house pay 25 Dollars, For each hotel pay 100 Dollars
        int hotelCount = 0;
        int houseCount = 0;

        for(int i = 0; i < interactingPlayer.ownedProperties.Count; i++)
        {
            houseCount += interactingPlayer.ownedProperties[i].houseCount;
            hotelCount += interactingPlayer.ownedProperties[i].hotelCount;
        }

        interactingPlayer.cash -= 25 * houseCount;
        interactingPlayer.cash -= 100 * hotelCount;
    }

    void AdvanceToKingsCross()
    {
        manager.StartCoroutine(manager.UpdateUIText("Take a ride to King’s Cross Station. If you pass Go, collect $200", 2));
        // Take a ride to King’s Cross Station. If you pass Go, collect 200 Dollars
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(5, true));
    }

    void AdvanceToMayfair()
    {
        manager.StartCoroutine(manager.UpdateUIText("Take a walk on the board walk. Advance token to Mayfair", 2));
        // Take a walk on the board walk. Advance token to Mayfair
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(39, true));
    }

    void PayPlayers()
    {
        manager.StartCoroutine(manager.UpdateUIText("You have been elected Chairman of the Board. Pay each player $50.", 2));
        // You have been elected Chairman of the Board. Pay each player 50 Dollars.
        for (int i = 0; i < manager.numberOfPlayers; i++)
        {
            // Pay each player that isnt the interacting player
            if (manager.players[i] != interactingPlayer)
            {
                interactingPlayer.cash -= 50;
                manager.players[i].cash += 50;
            }
        }
    }

    void LoanMatures()
    {
        manager.StartCoroutine(manager.UpdateUIText("Your building loan matures. Collect $150.", 2));
        // Your building loan matures. Collect 150 Dollars.
        interactingPlayer.cash += 150;
    }
}
