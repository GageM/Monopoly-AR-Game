using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpace : MonoBehaviour
{
    public Player interactingPlayer;
    public virtual void OnLanded(Player _interactingPlayer)
    {
        interactingPlayer = _interactingPlayer;
    }
}
