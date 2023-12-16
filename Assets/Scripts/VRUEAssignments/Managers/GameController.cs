using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject welcomePanel;
    public GameObject exitGamePanel;
    public GameObject inventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonPressed()
    {
        welcomePanel.SetActive(false);
    }

    public void OnStartEnemyWaveButtonPressed()
    {

    }


    public void OnExitGameButtonPressd()
    {
        exitGamePanel.SetActive(true);
        inventory.SetActive(false);
    }

    public void OnExitGameConfirmed()
    {
        Application.Quit();
    }

    public void OnExitGameCanceled()
    {
        exitGamePanel.SetActive(false);
        inventory.SetActive(true);
    }
}
