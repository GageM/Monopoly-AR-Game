using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperTax : PlayerSpace
{
    [SerializeField, Tooltip("How much money a player landing on this space will lose")]
    float taxAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnLanded(Player interactingPlayer)
    {
        interactingPlayer.cash -= taxAmount;
    }
}
