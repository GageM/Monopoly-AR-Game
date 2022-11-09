using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : Property
{
    [HideInInspector]
    public float diceMultiplier;

    public override bool CheckIfCanBuyHouse()
    {
        return false;
    }

    public override float ChooseRent()
    {
        if(siblingProperties[0].owner == owner)
        {
            diceMultiplier = 10;
        }
        else
        {
            diceMultiplier = 4;
        }
        return diceMultiplier * rent[0];
    }
}
