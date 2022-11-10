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
        chanceCards.Add(AdvanceToGo); //Works
        chanceCards.Add(AdvanceToTrafalgarSquare); //Works
        chanceCards.Add(AdvanceToPallMall); //Works
        chanceCards.Add(AdvanceToNearestRailroad); //Works
        chanceCards.Add(BankDividend); //Works
        chanceCards.Add(GetOutOfJailFree); //Works
        chanceCards.Add(GoToJail);
        chanceCards.Add(AdvanceToKingsCross); //Works
        chanceCards.Add(AdvanceToMayfair); //Works
        chanceCards.Add(LoanMatures); // Works
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
        manager.uIController.endTurnTitle.text = "Advance to Go, Collect $200";
        // Advance to Go, Collect 200 Dollars
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(0, true));
    }

    void AdvanceToTrafalgarSquare()
    {
        manager.uIController.endTurnTitle.text = "Advance to Trafalgar Square. If you pass Go, collect $200";
        // Advance to Trafalgar Square. If you pass Go, collect 200 Dollars
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(24, true));
    }

    void AdvanceToPallMall()
    {
        manager.uIController.endTurnTitle.text = "Advance to Pall Mall If you pass Go, collect $200.";
        // Advance to Pall Mall If you pass Go, collect 200 Dollars.
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(11, true));
    }
    void AdvanceToNearestRailroad()
    {
        manager.uIController.endTurnTitle.text = "Advance to the nearest Railroad.";
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
        manager.uIController.endTurnTitle.text = "Bank pays you dividend of $50";
        // Bank pays you dividend of 50 Dollars
        interactingPlayer.cash += 50;
    }

    void GetOutOfJailFree()
    {
        manager.uIController.endTurnTitle.text = "Get out of Jail Free";

        // add a get out of jail free card to the player
        interactingPlayer.getOutOfJailCards++;
    }

    void GoToJail()
    {
        manager.uIController.endTurnTitle.text = "Go directly to Jail. Do not pass Go, do not collect $200";
        // Go directly to Jail. Do not pass Go, do not collect 200 Dollars

        interactingPlayer.isInJail = true;
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(40, true));
    }

    void AdvanceToKingsCross()
    {
        manager.uIController.endTurnTitle.text = "Take a ride to King’s Cross Station. If you pass Go, collect $200";
        // Take a ride to King’s Cross Station. If you pass Go, collect 200 Dollars
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(5, true));
    }

    void AdvanceToMayfair()
    {
        manager.uIController.endTurnTitle.text = "Take a walk on the board walk. Advance token to Mayfair";
        // Take a walk on the board walk. Advance token to Mayfair
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(39, true));
    }

    void LoanMatures()
    {
        manager.uIController.endTurnTitle.text = "Your building loan matures. Collect $150.";
        // Your building loan matures. Collect 150 Dollars.
        interactingPlayer.cash += 150;
    }
}
