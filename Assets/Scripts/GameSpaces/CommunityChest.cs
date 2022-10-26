using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityChest : PlayerSpace
{
    GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("AR Session Origin").GetComponent<GameManager>();
    }

    public override void OnLanded(Player interactingPlayer)
    {
        base.OnLanded(interactingPlayer);
    }
}
