using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToJail : PlayerSpace
{
    public override void OnLanded(Player _interactingPlayer)
    {
        base.OnLanded(_interactingPlayer);

        interactingPlayer.gameManager.uIController.OpenGoToJailButton();
        interactingPlayer.gameManager.uIController.CloseEndTurnButton();
    }

    public override void SpaceInteraction()
    {
        interactingPlayer.isInJail = true;
        interactingPlayer.StartCoroutine(interactingPlayer.MoveToSpace(40, false));

        interactingPlayer.gameManager.uIController.CloseGoToJailButton();
        interactingPlayer.gameManager.uIController.OpenEndTurnButton();
    }
}
