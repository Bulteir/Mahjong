using Firebase;
using System;
using System.Collections.Generic;
using TMPro;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
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
    public GameObject menuBackground;

    public GameObject RateBox;
    public List<GameObject> CoinBarText;

    public Transform musics;
    public Transform sfx;

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

        //Oyun baþlangýcýnda kullanýcnýnýn toplam coin'i save dosyasýndan okunur
        SaveDataFormat saveFile = GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            foreach (GameObject coinBar in CoinBarText)
            {
                coinBar.GetComponent<CoinBar_Controller>().CoinBarText.text = saveFile.totalCoin.ToString();
            }
        }
        else//kayýt dosyasý hiç oluþturulmamýþ. 
        {
            foreach (GameObject coinBar in CoinBarText)
            {
                coinBar.GetComponent<CoinBar_Controller>().CoinBarText.text = GlobalVariables.FirstTotalCoin.ToString();
            }
        }

        #region firebase init iþlemi
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                FirebaseApp app = FirebaseApp.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        #endregion

        #region oyun baþlangýcý müzik tercihi kontrolü
        string musicPref = PlayerPrefs.GetString("Music");
        if (musicPref != "")
        {
            if (musicPref == "on")
            {
                foreach (Transform item in musics)
                {
                    item.GetComponent<AudioSource>().mute = false;
                }
            }
            else
            {
                foreach (Transform item in musics)
                {
                    item.GetComponent<AudioSource>().mute = true;
                }
            }
        }
        else
        {
            PlayerPrefs.SetString("Music", "on");
            foreach (Transform item in musics)
            {
                item.GetComponent<AudioSource>().mute = false;
            }
        }
        PlayerPrefs.Save();
        #endregion

        #region oyun baþlangýcý ses efektleri tercihi kontrolü
        string soundPref = PlayerPrefs.GetString("Sound");
        if (soundPref != "")
        {
            if (soundPref == "on")
            {
                foreach (Transform item in sfx)
                {
                    item.GetComponent<AudioSource>().mute = false;
                }
            }
            else
            {
                foreach (Transform item in sfx)
                {
                    item.GetComponent<AudioSource>().mute = true;
                }
            }
        }
        else
        {
            PlayerPrefs.SetString("Sound", "on");
            foreach (Transform item in sfx)
            {
                item.GetComponent<AudioSource>().mute = false;
            }
        }
        PlayerPrefs.Save();
        #endregion
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
            menuBackground.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_LevelSelectmenu && levelSelectMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            levelSelectMenu.SetActive(true);
            settingsMenu.SetActive(false);
            storeMenu.SetActive(false);
            leaderboardMenu.SetActive(false);
            menuBackground.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_SettingsMenu && settingsMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            levelSelectMenu.SetActive(false);
            settingsMenu.SetActive(true);
            storeMenu.SetActive(false);
            leaderboardMenu.SetActive(false);
            menuBackground.SetActive(false);
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
            menuBackground.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_LeaderboardMenu && leaderboardMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            levelSelectMenu.SetActive(false);
            settingsMenu.SetActive(false);
            storeMenu.SetActive(false);
            leaderboardMenu.SetActive(true);
            menuBackground.SetActive(false);
            //Leaderboard sayfasý açýldýðýnda scroll en baþta çýksýn diye
            //leaderboardMenu.GetComponentInChildren<Scrollbar>().value = 1;
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_MenuBackground && menuBackground.activeSelf == false)
        {
            mainMenu.SetActive(false);
            levelSelectMenu.SetActive(false);
            settingsMenu.SetActive(false);
            storeMenu.SetActive(false);
            leaderboardMenu.SetActive(false);
            menuBackground.SetActive(true);
        }
    }
}
