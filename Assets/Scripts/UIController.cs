using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] Image mainMenuUI;
    [SerializeField] Image playerCashPanel;
    [SerializeField] Image topPanel;
    [SerializeField] GameManager gameManager;

    // Debug Text on all screens
    [SerializeField] TextMeshProUGUI debugText;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNumberOfPlayers(int _numberOfPlayers)
    {
        gameManager.SetNumberOfPlayers(_numberOfPlayers);
    }

    public void StartGame()
    {
        mainMenuUI.gameObject.SetActive(false);
        mainMenuUI.enabled = false;

        playerCashPanel.gameObject.SetActive(true);
        topPanel.gameObject.SetActive(true);

        gameManager.StartAR();
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        playerCashPanel.gameObject.SetActive(false);
        topPanel.gameObject.SetActive(false);
    }
}
