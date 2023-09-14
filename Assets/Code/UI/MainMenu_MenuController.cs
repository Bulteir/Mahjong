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

        //Oyun ba�lang�c�nda kullan�cn�n�n toplam coin'i save dosyas�ndan okunur
        SaveDataFormat saveFile = GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kay�tl� save dosyas� varsa
        {
            foreach (GameObject coinBar in CoinBarText)
            {
                coinBar.GetComponent<CoinBar_Controller>().CoinBarText.text = saveFile.totalCoin.ToString();
            }
        }
        else//kay�t dosyas� hi� olu�turulmam��. 
        {
            foreach (GameObject coinBar in CoinBarText)
            {
                coinBar.GetComponent<CoinBar_Controller>().CoinBarText.text = GlobalVariables.FirstTotalCoin.ToString();
            }
        }

        #region firebase init i�lemi
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

        #region oyun ba�lang�c� m�zik tercihi kontrol�
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

        #region oyun ba�lang�c� ses efektleri tercihi kontrol�
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
            //store sayfas� a��ld���nda scroll en ba�ta ��ks�n diye
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
            //Leaderboard sayfas� a��ld���nda scroll en ba�ta ��ks�n diye
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
