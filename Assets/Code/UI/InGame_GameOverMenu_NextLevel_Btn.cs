using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class InGame_GameOverMenu_NextLevel_Btn : MonoBehaviour
{
    public GameObject generalControllers;
    public GameObject nextLevelInfoPopup;
    public GameObject adMobControllers;
    public void OnClick()
    {
        bool noAdsJokerActive = false;
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kay�tl� save dosyas� varsa
        {
            if (saveFile.noAdsJokerActive == true)//kullan�c� no ads e�yas� al�nm��sa.
            {
                noAdsJokerActive = true;
            }
        }

        if (noAdsJokerActive)
        {
            if (generalControllers.GetComponent<EnergyBarController>().IsThereEnoughEnergyForLevel())
            {
                GoNextLevel();
            }
        }
        else
        {
            //sonraki b�l�me ge�mek i�in yeterli enerji varsa
            if (generalControllers.GetComponent<EnergyBarController>().IsThereEnoughEnergyForLevel())
            {
                GlobalVariables.intersitialAd_CallingObject = gameObject;
                adMobControllers.GetComponent<InterstitialAdController>().ShowAd();
            }
        }
    }

    public void NextLevel()
    {
        if (GlobalVariables.intersitialAd_CallingObject == gameObject)
        {
            GlobalVariables.intersitialAd_CallingObject = null;
            adMobControllers.GetComponent<InterstitialAdController>().DestroyAd();
            adMobControllers.GetComponent<BannerViewController>().DestroyAd();

            GoNextLevel();
        }
    }

    void GoNextLevel()
    {
        LevelProperties levelProperties = new LevelProperties();
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kay�tl� save dosyas� varsa
        {
            //bir sonraki b�l�m�n numaras�
            int levelNumber = Int32.Parse(SceneManager.GetActiveScene().name.Replace("level", "")) + 1;
            //bir sonraki b�l�m�n ad�
            string levelName = "level" + levelNumber.ToString();
            levelProperties = saveFile.levelProperties.Where(i => i.LevelName == levelName).FirstOrDefault();


            if (levelProperties.LevelName != null && levelProperties.levelPurchased) //bir sonraki b�l�m daha �nceden sat�n al�nm��.
            {
                SceneManager.LoadScene("level" + levelNumber, LoadSceneMode.Single);
            }
            else if (saveFile.totalCoin >= GlobalVariables.LevelRewards[levelNumber - 1] * 3)//b�l�m� sat�n almak i�in yeterli alt�n var
            {
                saveFile.totalCoin = saveFile.totalCoin - (GlobalVariables.LevelRewards[levelNumber - 1] * 3);

                saveFile.levelProperties.Remove(levelProperties);

                //a��lacak b�l�me ait bir kay�t yoktur.
                if (levelProperties.LevelName == null)
                {
                    levelProperties.LevelName = "level" + levelNumber.ToString();
                    levelProperties.levelPassed = false;
                }

                levelProperties.levelPurchased = true;
                saveFile.levelProperties.Add(levelProperties);

                generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);

                generalControllers.GetComponent<InGame_MenuController>().CoinBarText.GetComponent<CoinBar_Controller>().AddRemoveCoin(-(GlobalVariables.LevelRewards[levelNumber - 1] * 3), saveFile.totalCoin);
            }
            else//yeterli alt�n yok
            {
                nextLevelInfoPopup.GetComponent<InGame_NextLevelInfoPopup_Controller>().ShowPopup(levelNumber);
            }
        }
    }
}
