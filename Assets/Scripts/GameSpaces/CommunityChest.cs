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
        communityCards.Add(StreetRepairs);
        communityCards.Add(SecondPrize);
        communityCards.Add(Inheritance);
    }


    public override void OnLanded(Player interactingPlayer)
    {
        base.OnLanded(interactingPlayer);

        int topCard = Random.Range(0, communityCards.Count - 1);

        communityCards[topCard]();
    }

    void AdvanceToGo()
    {
        manager.StartCoroutine(manager.UpdateUIText("Advance to Go, Collect $200", 2));

        manager.StartCoroutine(manager.MovePlayerToSpace(interactingPlayer.playerIndex, 0, true));
    }

    void BankError()
    {
        manager.StartCoroutine(manager.UpdateUIText("Bank error in your favour. Collect $200", 2));

        interactingPlayer.cash += 200;
    }

    void DoctorsFees()
    {
        manager.StartCoroutine(manager.UpdateUIText("Doctors fees. Pay $50", 2));

        interactingPlayer.cash -= 50;
    }

    void GetOutOfJailFree()
    {
        manager.StartCoroutine(manager.UpdateUIText("Get out of Jail Free", 2));
        // TODO: Set Up Jail System
    }

    void GoToJail()
    {
        manager.StartCoroutine(manager.UpdateUIText("Go directly to Jail. Do not pass Go, do not collect $200", 2));
        // Go directly to Jail. Do not pass Go, do not collect 200 Dollars
        manager.StartCoroutine(manager.MovePlayerToSpace(interactingPlayer.playerIndex, 40, false));
    }

    void OperaNight()
    {
        manager.StartCoroutine(manager.UpdateUIText("Grand opera night. Collect $50 from every player for opening night seats", 2));

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
        manager.StartCoroutine(manager.UpdateUIText("Holiday fund matures. Collect $100", 2));
        interactingPlayer.cash += 100;
    }

    void TaxRefund()
    {
        manager.StartCoroutine(manager.UpdateUIText("Income tax refund. Collect $20", 2));
        interactingPlayer.cash += 20;
    }

    void Birthday()
    {
        manager.StartCoroutine(manager.UpdateUIText("It's your birthday. Collect $10 from every player", 2));

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
        manager.StartCoroutine(manager.UpdateUIText("Life insurance matures. Collect $100", 2));

        interactingPlayer.cash += 100;
    }

    void HospitalFees()
    {
        manager.StartCoroutine(manager.UpdateUIText("Hospital fees. Pay $50", 2));

        interactingPlayer.cash -= 50;
    }

    void SchoolFees()
    {
        manager.StartCoroutine(manager.UpdateUIText("School fees. Pay $50", 2));

        interactingPlayer.cash -= 50;
    }

    void ConsultancyFees()
    {
        manager.StartCoroutine(manager.UpdateUIText("Recieve $25 consultancy fee", 2));

        interactingPlayer.cash -= 25;
    }

    void StreetRepairs()
    {
        manager.StartCoroutine(manager.UpdateUIText("You are assessed for street repairs. Pay $40 per house and $115 for each hotel you own", 2));
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
        manager.StartCoroutine(manager.UpdateUIText("You won 2nd place in a beauty contest. Collect $10", 2));

        interactingPlayer.cash += 10;
    }

    void Inheritance()
    {
        manager.StartCoroutine(manager.UpdateUIText("You inherit $100", 2));

        interactingPlayer.cash += 100;
    }

}
