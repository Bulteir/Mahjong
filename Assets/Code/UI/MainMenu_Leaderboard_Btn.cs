using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class MainMenu_Leaderboard_Btn : MonoBehaviour
{
    public GameObject leaderboardController;
    public GameObject generalControllers;
    public void OnClick()
    {
        //leaderboard men�s� a��l�rken save dosyas� yoksa default save dosyas� olu�turulur.
        generalControllers.GetComponent<GameSaveLoadController>().CreateDefaultSaveFile();

        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kay�tl� save dosyas� varsa
        {
            leaderboardController.GetComponent<LeaderboardController>().AddScore(saveFile.totalCoin);
        }
        GlobalVariables.gameState = GlobalVariables.gameState_LeaderboardMenu;
    }
}
