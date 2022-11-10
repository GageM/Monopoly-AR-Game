using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperTax : PlayerSpace
{
    [SerializeField, Tooltip("How much money a player landing on this space will lose")]
    float taxAmount;

    public override void OnLanded(Player _interactingPlayer)
    {
        base.OnLanded(_interactingPlayer);
        
        if(interactingPlayer.cash >= taxAmount)
        {
            interactingPlayer.gameManager.uIController.OpenPayTaxButton(taxAmount);
        }
        else
        {
            interactingPlayer.gameManager.uIController.OpenGiveUpButton();
        }
        interactingPlayer.gameManager.uIController.CloseEndTurnButton();
    }

    public override void SpaceInteraction()
    {
        interactingPlayer.cash -= taxAmount;

        interactingPlayer.gameManager.uIController.ClosePayTaxButton();
        interactingPlayer.gameManager.uIController.OpenEndTurnButton();
    }
}
