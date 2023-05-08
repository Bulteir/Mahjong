using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class MainMenu_MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelectMenu;
    public GameObject settingsMenu;
    public GameObject storeMenu;
    public GameObject leaderboardMenu;
    
    public GameObject RateBox;

    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        menuActiveControl();
        RateBox.GetComponent<RateBoxController>().RateBoxControlAtStartGame();

        string selectedLangVal = PlayerPrefs.GetString("SelectedLang");
        if (selectedLangVal != "")
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[int.Parse(selectedLangVal)];
        }

        //burada amacýmýz kullanýcý ana menüye geldiðinde verilerini buluta senkronize etmek. Levelden ana menüye geldiðinde de çalýþýr.
        //oyun ilk açýldýðýnda çalýþmaz. Ýstediðimzide bu. Oyun ilk açýldýðýnda authantication iþlemlerinin bitiminde otomatik çaðrýlýr.
        //Ayrýca bulut hizmetlerinine okuma yazma yükünü hafifletmek için oyun açýldýktan sonra otantike olduðunda bir kez senkronize olur.
        //Bu senkronizasyon haricinde ana menüye bir kez daha geldiðinde son bir kez daha senkronize olur.
        //Yani oyuna baþlayýnca otantike iþlemi bitince birkez senkron olur. sonra oynadýk ana menüye döndük diyelim son bir kez daha senkron olur.
        if (GlobalVariables.cloudSaveSystemIsReady == true && GlobalVariables.firstSynchronisationOfGame == true)
        {
            GetComponent<GameSaveLoadController>().GameSaveDataSynchronization();
            GlobalVariables.firstSynchronisationOfGame = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        menuActiveControl();
    }

    void menuActiveControl()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_MainMenu && mainMenu.activeSelf == false)
        {
            mainMenu.SetActive(true);
            levelSelectMenu.SetActive(false);
            settingsMenu.SetActive(false);
            storeMenu.SetActive(false);
            leaderboardMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_LevelSelectmenu && levelSelectMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            levelSelectMenu.SetActive(true);
            settingsMenu.SetActive(false);
            storeMenu.SetActive(false);
            leaderboardMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_SettingsMenu && settingsMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            levelSelectMenu.SetActive(false);
            settingsMenu.SetActive(true);
            storeMenu.SetActive(false);
            leaderboardMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_StoreMenu && storeMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            levelSelectMenu.SetActive(false);
            settingsMenu.SetActive(false);
            storeMenu.SetActive(true);
            //store sayfasý açýldýðýnda scroll en baþta çýksýn diye
            storeMenu.GetComponentInChildren<Scrollbar>().value = 1;
            leaderboardMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_LeaderboardMenu && leaderboardMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            levelSelectMenu.SetActive(false);
            settingsMenu.SetActive(false);
            storeMenu.SetActive(false);
            leaderboardMenu.SetActive(true);
            leaderboardMenu.GetComponent<LeaderboardController>().FillLeaderboardList();
            //Leaderboard sayfasý açýldýðýnda scroll en baþta çýksýn diye
            leaderboardMenu.GetComponentInChildren<Scrollbar>().value = 1;
        }
    }
}
