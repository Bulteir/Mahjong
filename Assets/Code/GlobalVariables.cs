using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    public const string TagBlock = "Block";

    public static int gameState = gameState_inGame;
    public const int gameState_MainMenu = 0;
    public const int gameState_inGame = 1;
    public const int gameState_SettingsMenu = 2;
    public const int gameState_HighScoresMenu = 3;
    public const int gameState_LoadingGame = 4;
    public const int gameState_gamePaused = 5;
    public const int gameState_gameOver = 6;
}
