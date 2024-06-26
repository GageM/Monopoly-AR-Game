using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpace : MonoBehaviour
{
    [HideInInspector]
    public Player interactingPlayer;
    public virtual void OnLanded(Player _interactingPlayer)
    {
        interactingPlayer = _interactingPlayer;
        interactingPlayer.gameManager.uIController.OpenEndTurnButton();
    }

    public virtual void SpaceInteraction()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Ray(transform.position, transform.forward));
    }
}
