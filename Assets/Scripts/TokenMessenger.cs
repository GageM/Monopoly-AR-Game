using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenMessenger : MonoBehaviour
{
    public void StartMoving()
    {
        GetComponentInParent<Player>().StartMoving();
    }
}
