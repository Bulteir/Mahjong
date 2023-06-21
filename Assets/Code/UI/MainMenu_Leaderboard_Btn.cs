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
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            leaderboardController.GetComponent<LeaderboardController>().AddScore(saveFile.totalCoin);
        }
        GlobalVariables.gameState = GlobalVariables.gameState_LeaderboardMenu;
    }
}
