using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeTax : PlayerSpace
{
    [SerializeField, Tooltip("The percent of the player's net worth to be taken as tax")]
    float taxPercentage;

    [SerializeField, Tooltip("The default tax amount")]
    float flatTaxAmount;

    [HideInInspector, Tooltip("The amount of money taken as tax")]
    float taxAmount;

    // Start is called before the first frame update
    void Start()
    {
        taxPercentage /= 100f;
    }

    public override void OnLanded(Player _interactingPlayer)
    {

        base.OnLanded(_interactingPlayer);
        float netWorth = 0;

        for(int i = 0; i < interactingPlayer.ownedProperties.Count; i++)
        {
            netWorth += interactingPlayer.ownedProperties[i].value;
        }
        netWorth += interactingPlayer.cash;

        taxAmount = netWorth * taxPercentage;

        interactingPlayer.cash -= (taxAmount <= 200f) ? taxAmount : flatTaxAmount;

    }
}
