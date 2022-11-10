using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyToggle : MonoBehaviour
{
    [HideInInspector] public bool willSell;
    [HideInInspector] public Property property;
    [HideInInspector] public TradeMenu menu;

    public void ToggleWillSell(bool _willSell)
    {
        willSell = _willSell;

        if(willSell)
        {
            menu.AddToPlayerProperties(property, property.owner);
        }
        else
        {
            menu.RemoveFromPlayerProperties(property, property.owner);
        }
    }
}
