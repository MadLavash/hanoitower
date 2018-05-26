using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject finalMenu;

    private void Awake()
    {
        GameController.gameFinished += GameFinished;

        mainMenu.SetActive(true);
        finalMenu.SetActive(false);
    }

    private void GameFinished()
    {
        mainMenu.SetActive(false);
        finalMenu.SetActive(true);
    }

    private void OnDestroy()
    {
        GameController.gameFinished -= GameFinished;
    }
}
