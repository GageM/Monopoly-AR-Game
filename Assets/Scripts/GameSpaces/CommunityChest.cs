using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityChest : PlayerSpace
{
    GameManager manager;

    delegate bool CommunityCards();

    readonly List<CommunityCards> communityCards = new();

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("AR Session Origin").GetComponent<GameManager>();

        communityCards.Add(AdvanceToGo);
        communityCards.Add(BankError);
        communityCards.Add(DoctorsFees);
        communityCards.Add(GetOutOfJailFree);
        communityCards.Add(GoToJail);
        communityCards.Add(OperaNight);
        communityCards.Add(FundMatures);
        communityCards.Add(TaxRefund);
        communityCards.Add(Birthday);
        communityCards.Add(InsuranceMatures);
        communityCards.Add(HospitalFees);
        communityCards.Add(SchoolFees);
        communityCards.Add(ConsultancyFees);
        communityCards.Add(SecondPrize);
        communityCards.Add(Inheritance);
    }


    public override void OnLanded(Player interactingPlayer)
    {
        base.OnLanded(interactingPlayer);

        interactingPlayer.gameManager.uIController.OpenDrawCardButton();
        interactingPlayer.gameManager.uIController.CloseEndTurnButton();
    }

    public override void SpaceInteraction()
    {
        // Choose a random card from the deck
        int topCard = Random.Range(0, communityCards.Count - 1);

        // Check if the card was completed
        bool cardComplete;

        // Call that card's function
        cardComplete = communityCards[topCard]();


        interactingPlayer.gameManager.uIController.CloseDrawCardButton();

        if (cardComplete)
        {
            interactingPlayer.gameManager.uIController.OpenEndTurnButton();
        }
        else
        {
            interactingPlayer.gameManager.uIController.OpenGiveUpButton();
        }
    }

    bool AdvanceToGo()
    {
        manager.uIController.endTurnTitle.text = "Advance to Go, Collect $200";

        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(0, true));

        return true;
    }

    bool BankError()
    {
        manager.uIController.endTurnTitle.text = "Bank error in your favour. Collect $200";

        interactingPlayer.cash += 200;

        return true;
    }

    bool DoctorsFees()
    {
        manager.uIController.endTurnTitle.text = "Doctors fees. Pay $50";
        if (interactingPlayer.cash >= 50)
        {
            interactingPlayer.cash -= 50;
            return true;
        }
        else return false;
    }

    bool GetOutOfJailFree()
    {
        manager.uIController.endTurnTitle.text = "Get out of Jail Free";

        // add a get out of jail free card to the player
        interactingPlayer.getOutOfJailCards++;

        return true;
    }

    bool GoToJail()
    {
        manager.uIController.endTurnTitle.text = "Go directly to Jail. Do not pass Go, do not collect $200";
        // Go directly to Jail. Do not pass Go, do not collect 200 Dollars

        interactingPlayer.isInJail = true;
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(40, true));

        return true;
    }

    bool OperaNight()
    {
        manager.uIController.endTurnTitle.text = "Grand opera night. Collect $50 from every player for opening night seats";

        for (int i = 0; i < manager.numberOfPlayers - 1; i++)
        {
            if(manager.players[i] != interactingPlayer && manager.players[i].cash >= 50)
            {
                manager.players[i].cash -= 50;
                interactingPlayer.cash += 50;
            }
        }
        return true;
    }

    bool FundMatures()
    {
        manager.uIController.endTurnTitle.text = "Holiday fund matures. Collect $100";
        interactingPlayer.cash += 100;

        return true;
    }

    bool TaxRefund()
    {
        manager.uIController.endTurnTitle.text = "Income tax refund. Collect $20";
        interactingPlayer.cash += 20;

        return true;
    }

    bool Birthday()
    {
        manager.uIController.endTurnTitle.text = "It's your birthday. Collect $10 from every player";

        for (int i = 0; i < manager.numberOfPlayers - 1; i++)
        {
            if (manager.players[i] != interactingPlayer && manager.players[i].cash >= 10)
            {
                manager.players[i].cash -= 10;
                interactingPlayer.cash += 10;
            }
        }
        return true;
    }

    bool InsuranceMatures()
    {
        manager.uIController.endTurnTitle.text = "Life insurance matures. Collect $100";

        interactingPlayer.cash += 100;

        return true;
    }

    bool HospitalFees()
    {
        manager.uIController.endTurnTitle.text = "Hospital fees. Pay $50";

        interactingPlayer.cash -= 50;

        if(interactingPlayer.cash < 0) { return false; }
        else { return true; }
    }

    bool SchoolFees()
    {
        manager.uIController.endTurnTitle.text = "School fees. Pay $50";

        interactingPlayer.cash -= 50;

        if(interactingPlayer.cash < 0)
        {
            return false;
        }
        else { return true; }
    }

    bool ConsultancyFees()
    {
        manager.uIController.endTurnTitle.text = "Recieve $25 consultancy fee";

        interactingPlayer.cash -= 25;

        if(interactingPlayer.cash < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool SecondPrize()
    {
        manager.uIController.endTurnTitle.text = "You won 2nd place in a beauty contest. Collect $10";

        interactingPlayer.cash += 10;

        return true;
    }

    bool Inheritance()
    {
        manager.uIController.endTurnTitle.text = "You inherit $100";

        interactingPlayer.cash += 100;

        return true;
    }

}
