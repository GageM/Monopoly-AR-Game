using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chance : PlayerSpace
{
    GameManager manager;

    delegate void ChanceCards();

    List<ChanceCards> chanceCards = new();

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("AR Session Origin").GetComponent<GameManager>();

        // Add the chance cards to the list
        chanceCards.Add(AdvanceToGo);
        chanceCards.Add(AdvanceToTrafalgarSquare);
        chanceCards.Add(AdvanceToPallMall);
        chanceCards.Add(AdvanceToNearestRailroad);
        chanceCards.Add(AdvanceToUtility);
        chanceCards.Add(BankDividend);
        chanceCards.Add(GetOutOfJailFree);
        chanceCards.Add(GoBackSpaces);
        chanceCards.Add(GoToJail);
        chanceCards.Add(PropertyRepair);
        chanceCards.Add(AdvanceToKingsCross);
        chanceCards.Add(AdvanceToMayfair);
        chanceCards.Add(PayPlayers);
        chanceCards.Add(LoanMatures);
    }

    public override void OnLanded(Player _interactingPlayer)
    {
        base.OnLanded(_interactingPlayer);
        int topCard = Random.Range(0, chanceCards.Count - 1);
        chanceCards[topCard]();
    }

    void AdvanceToGo()
    {
        manager.StartCoroutine(manager.UpdateUIText("Advance to Go, Collect 200 Dollars", 2));
        // Advance to Go, Collect 200 Dollars
        manager.StartCoroutine(manager.MovePlayerToSpace(interactingPlayer.playerIndex, 0, true));
    }

    void AdvanceToTrafalgarSquare()
    {
        manager.StartCoroutine(manager.UpdateUIText("Advance to Trafalgar Square. If you pass Go, collect 200 Dollars", 2));
        // Advance to Trafalgar Square. If you pass Go, collect 200 Dollars
        manager.StartCoroutine(manager.MovePlayerToSpace(interactingPlayer.playerIndex, 24, true));
    }

    void AdvanceToPallMall()
    {
        manager.StartCoroutine(manager.UpdateUIText("Advance to Pall Mall If you pass Go, collect 200 Dollars.", 2));
        // Advance to Pall Mall If you pass Go, collect 200 Dollars.
        manager.StartCoroutine(manager.MovePlayerToSpace(interactingPlayer.playerIndex, 11, true));
    }

    void AdvanceToUtility()
    {
        manager.StartCoroutine(manager.UpdateUIText("Advance token to the nearest Utility. If unowned, you may buy it from the Bank. If owned, throw dice and pay owner a total 10 times the amount thrown.", 2));
        // Advance token to the nearest Utility. If unowned, you may buy it from the Bank. If owned, throw dice and pay owner a total 10 times the amount thrown.
    }

    void AdvanceToNearestRailroad()
    {
        manager.StartCoroutine(manager.UpdateUIText("Advance to the nearest Railroad. If unowned, you may buy it from the Bank. If owned, pay owner twice the rental price.", 2));
        // Advance to the nearest Railroad. If unowned, you may buy it from the Bank. If owned, pay owner twice the rental price.
    }

    void BankDividend()
    {
        manager.StartCoroutine(manager.UpdateUIText("Bank pays you dividend of 50 Dollars", 2));
        // Bank pays you dividend of 50 Dollars
        interactingPlayer.cash += 50;
    }

    void GetOutOfJailFree()
    {
        manager.StartCoroutine(manager.UpdateUIText("Get out of Jail Free", 2));
        // TODO: Set Up Jail System
    }

    void GoBackSpaces()
    {
        manager.StartCoroutine(manager.UpdateUIText("Go back 3 spaces", 2));
        // Go back 3 spaces

        // Set the player's new space to 3 spaces behind their current space
        int newSpaceIndex = (interactingPlayer.currentSpaceIndex <= 3) ? (interactingPlayer.currentSpaceIndex - 3) : (interactingPlayer.currentSpaceIndex + 39 - 3);

        // Move the player to their new space
        manager.StartCoroutine(manager.MovePlayerToSpace(interactingPlayer.playerIndex, newSpaceIndex, false));
    }

    void GoToJail()
    {
        manager.StartCoroutine(manager.UpdateUIText("Go directly to Jail. Do not pass Go, do not collect 200 Dollars", 2));
        // Go directly to Jail. Do not pass Go, do not collect 200 Dollars
        manager.StartCoroutine(manager.MovePlayerToSpace(interactingPlayer.playerIndex, 40, false));
    }

    void PropertyRepair()
    {
        manager.StartCoroutine(manager.UpdateUIText("Make general repairs on all your properties: For each house pay 25 Dollars, For each hotel pay 100 Dollars", 2));
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
        manager.StartCoroutine(manager.UpdateUIText("Take a ride to King’s Cross Station. If you pass Go, collect 200 Dollars", 2));
        // Take a ride to King’s Cross Station. If you pass Go, collect 200 Dollars
        manager.StartCoroutine(manager.MovePlayerToSpace(interactingPlayer.playerIndex, 5, true));
    }

    void AdvanceToMayfair()
    {
        manager.StartCoroutine(manager.UpdateUIText("Take a walk on the board walk. Advance token to Mayfair", 2));
        // Take a walk on the board walk. Advance token to Mayfair
        manager.StartCoroutine(manager.MovePlayerToSpace(interactingPlayer.playerIndex, 39, true));
    }

    void PayPlayers()
    {
        manager.StartCoroutine(manager.UpdateUIText("You have been elected Chairman of the Board. Pay each player 50 Dollars.", 2));
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
        manager.StartCoroutine(manager.UpdateUIText("Your building loan matures. Collect 150 Dollars.", 2));
        // Your building loan matures. Collect 150 Dollars.
        interactingPlayer.cash += 150;
    }
}
