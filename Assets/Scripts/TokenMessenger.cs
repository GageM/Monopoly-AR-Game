using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenMessenger : MonoBehaviour
{
    public void StartMoving(float moveDuration)
    {
        GetComponentInParent<Player>().StartMoving(moveDuration);
    }
}
