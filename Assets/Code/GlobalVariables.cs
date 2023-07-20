using System;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    public static bool firstSynchronisationOfGame = true;
    public static bool internetAvaible = true;
    public static bool cloudSaveSystemIsReady = false;
    public const string TagBlock = "Block";


    public static int gameState = gameState_StartingLogo;
    public const int gameState_MainMenu = 0;
    public const int gameState_inGame = 1;
    public const int gameState_SettingsMenu = 2;
    public const int gameState_LeaderboardMenu = 3;
    public const int gameState_gamePaused = 4;
    public const int gameState_gameOver = 5;
    public const int gameState_LevelSelectmenu = 6;
    public const int gameState_StoreMenu = 7;
    public const int gameState_MenuBackground = 8;
    public const int gameState_StartingLogo = 9;
    public const int gameState_tutorial = 10;

    public const string LeaderboardId_Richest = "richestMonthly";

    public const int FirstTotalCoin = 1000;
    public const int FirstShuffleJokerQuantity = 5;
    public const int FirstUndoJokerQuantity = 5;

    public const int maxEnergy = 100;
    public const int requiredEnergyForlevel = 10;

    public static GameObject intersitialAd_CallingObject = null;
    public static GameObject rewardedAd_CallingObject = null;

    public static int MainMenuRewardAdType = MainMenuRewardAdType_Coin;
    public const int MainMenuRewardAdType_Coin = 0;
    public const int MainMenuRewardAdType_Shuffle = 1;
    public const int MainMenuRewardAdType_Undo = 2;

    public static bool tutorialCalledSettingMenu = false;


    //bölümlerin kaç altýn kazandýracaðý. Ayný zamanda o bölümün açýlmasý için ne kadar alýtýn gerektiði x3 olarak bulunur. 
    public static List<int> LevelRewards = new List<int>()
    {
        101,
        102,
        103,
        104,
        105,
        106,
    };

}
