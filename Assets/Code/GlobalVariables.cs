public static class GlobalVariables
{
    public static bool firstSynchronisationOfGame = true;
    public static bool internetAvaible = true;
    public static bool cloudSaveSystemIsReady = false;
    public const string TagBlock = "Block";


    public static int gameState = gameState_MainMenu;
    public const int gameState_MainMenu = 0;
    public const int gameState_inGame = 1;
    public const int gameState_SettingsMenu = 2;
    public const int gameState_LeaderboardMenu = 3;
    public const int gameState_gamePaused = 4;
    public const int gameState_gameOver = 5;
    public const int gameState_LevelSelectmenu = 6;
    public const int gameState_StoreMenu = 7;
}
