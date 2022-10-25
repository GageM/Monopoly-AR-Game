using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public static GameBoard Instance { get; private set; }

    [SerializeField, Tooltip("All of the Board's Spaces")]
    public PlayerSpace[] spaces;

    // Start is called before the first frame update
    public void Awake()
    {
        // Check if there is another game board to ensure only one in the scene
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
