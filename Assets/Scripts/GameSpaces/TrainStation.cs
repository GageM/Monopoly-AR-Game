using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainStation : Property
{

    public override bool CheckIfCanBuyHouse()
    {
        return false;
    }

    public override float ChooseRent()
    {
        rent[0] = 25;
        for(int i = 0; i < siblingProperties.Count; i++)
        {
            if(siblingProperties[i].owner == owner)
            {
                rent[0] += 25;
            }
        }
        return rent[0];
    }
}
