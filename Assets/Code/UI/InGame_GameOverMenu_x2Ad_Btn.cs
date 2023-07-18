using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_GameOverMenu_x2Ad_Btn : MonoBehaviour
{
    public GameObject AdMobController;
    public GameObject generalControllers;
    public AudioSource coinSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        GlobalVariables.rewardedAd_CallingObject = gameObject;
        AdMobController.GetComponent<RewardedAdController>().ShowAd();
    }

    public void GetBonusCoin()
    {
        if (GlobalVariables.rewardedAd_CallingObject == gameObject)
        {
            GlobalVariables.rewardedAd_CallingObject = null;

            SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
            if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
            {
                int currentLevelReward = GlobalVariables.LevelRewards[(Int32.Parse(SceneManager.GetActiveScene().name.Replace("level", "")) - 1)];
                saveFile.totalCoin += currentLevelReward;
                generalControllers.GetComponent<InGame_MenuController>().CoinBarText.GetComponent<CoinBar_Controller>().AddRemoveCoin(currentLevelReward, saveFile.totalCoin);
                saveFile.saveTime = DateTime.Now.ToString();
                generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
                coinSound.Play();
            }
        }
    }
}
