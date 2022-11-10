using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityChest : PlayerSpace
{
    GameManager manager;

    delegate void CommunityCards();

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
        //communityCards.Add(StreetRepairs);
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

        // Call that card's function
        communityCards[topCard]();

        interactingPlayer.gameManager.uIController.CloseDrawCardButton();
        interactingPlayer.gameManager.uIController.OpenEndTurnButton();
    }

    void AdvanceToGo()
    {
        manager.uIController.endTurnTitle.text = "Advance to Go, Collect $200";

        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(0, true));
    }

    void BankError()
    {
        manager.uIController.endTurnTitle.text = "Bank error in your favour. Collect $200";

        interactingPlayer.cash += 200;
    }

    void DoctorsFees()
    {
        manager.uIController.endTurnTitle.text = "Doctors fees. Pay $50";

        interactingPlayer.cash -= 50;
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

    void OperaNight()
    {
        manager.uIController.endTurnTitle.text = "Grand opera night. Collect $50 from every player for opening night seats";

        for (int i = 0; i < manager.numberOfPlayers - 1; i++)
        {
            if(manager.players[i] != interactingPlayer)
            {
                manager.players[i].cash -= 50;
                interactingPlayer.cash += 50;
            }
        }
    }

    void FundMatures()
    {
        manager.uIController.endTurnTitle.text = "Holiday fund matures. Collect $100";
        interactingPlayer.cash += 100;
    }

    void TaxRefund()
    {
        manager.uIController.endTurnTitle.text = "Income tax refund. Collect $20";
        interactingPlayer.cash += 20;
    }

    void Birthday()
    {
        manager.uIController.endTurnTitle.text = "It's your birthday. Collect $10 from every player";

        for (int i = 0; i < manager.numberOfPlayers - 1; i++)
        {
            if (manager.players[i] != interactingPlayer)
            {
                manager.players[i].cash -= 10;
                interactingPlayer.cash += 10;
            }
        }
    }

    void InsuranceMatures()
    {
        manager.uIController.endTurnTitle.text = "Life insurance matures. Collect $100";

        interactingPlayer.cash += 100;
    }

    void HospitalFees()
    {
        manager.uIController.endTurnTitle.text = "Hospital fees. Pay $50";

        interactingPlayer.cash -= 50;
    }

    void SchoolFees()
    {
        manager.uIController.endTurnTitle.text = "School fees. Pay $50";

        interactingPlayer.cash -= 50;
    }

    void ConsultancyFees()
    {
        manager.uIController.endTurnTitle.text = "Recieve $25 consultancy fee";

        interactingPlayer.cash -= 25;
    }

    void StreetRepairs()
    {
        manager.uIController.endTurnTitle.text = "You are assessed for street repairs. Pay $40 per house and $115 for each hotel you own";
        // Make general repairs on all your properties: For each house pay 25 Dollars, For each hotel pay 100 Dollars
        int hotelCount = 0;
        int houseCount = 0;

        for (int i = 0; i < interactingPlayer.ownedProperties.Count; i++)
        {
            houseCount += interactingPlayer.ownedProperties[i].houseCount;
            hotelCount += interactingPlayer.ownedProperties[i].hotelCount;
        }

        interactingPlayer.cash -= 40 * houseCount;
        interactingPlayer.cash -= 115 * hotelCount;
    }

    void SecondPrize()
    {
        manager.uIController.endTurnTitle.text = "You won 2nd place in a beauty contest. Collect $10";

        interactingPlayer.cash += 10;
    }

    void Inheritance()
    {
        manager.uIController.endTurnTitle.text = "You inherit $100";

        interactingPlayer.cash += 100;
    }

}
