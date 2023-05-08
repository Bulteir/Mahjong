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

        //burada amac�m�z kullan�c� ana men�ye geldi�inde verilerini buluta senkronize etmek. Levelden ana men�ye geldi�inde de �al���r.
        //oyun ilk a��ld���nda �al��maz. �stedi�imzide bu. Oyun ilk a��ld���nda authantication i�lemlerinin bitiminde otomatik �a�r�l�r.
        //Ayr�ca bulut hizmetlerinine okuma yazma y�k�n� hafifletmek i�in oyun a��ld�ktan sonra otantike oldu�unda bir kez senkronize olur.
        //Bu senkronizasyon haricinde ana men�ye bir kez daha geldi�inde son bir kez daha senkronize olur.
        //Yani oyuna ba�lay�nca otantike i�lemi bitince birkez senkron olur. sonra oynad�k ana men�ye d�nd�k diyelim son bir kez daha senkron olur.
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
            //store sayfas� a��ld���nda scroll en ba�ta ��ks�n diye
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
            //Leaderboard sayfas� a��ld���nda scroll en ba�ta ��ks�n diye
            leaderboardMenu.GetComponentInChildren<Scrollbar>().value = 1;
        }
    }
}
