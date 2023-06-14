using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_RewardedAd_Btn : MonoBehaviour
{
    public GameObject adMobControllers;
    public GameObject generalControllers;

    public Sprite CoinRewardImage;
    public Sprite ShuffleRewardImage;
    public Sprite UndoRewardImage;

    public int coinReward = 200;
    public int shuffleReward = 2;
    public int undoReward = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnClick()
    {
        GlobalVariables.rewardedAd_CallingObject = gameObject;
        adMobControllers.GetComponent<RewardedAdController>().ShowAd();
    }

    public void SetReward()
    {
        if (GlobalVariables.rewardedAd_CallingObject == gameObject)
        {
            GlobalVariables.rewardedAd_CallingObject = null;

            SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
            if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
            {

                if (GlobalVariables.MainMenuRewardAdType == GlobalVariables.MainMenuRewardAdType_Coin)
                {
                    saveFile.totalCoin += coinReward;

                    foreach (var coinBar in generalControllers.GetComponent<MainMenu_MenuController>().CoinBarText)
                    {
                        coinBar.GetComponent<CoinBar_Controller>().AddRemoveCoin(coinReward, saveFile.totalCoin);
                    }
                }
                else if (GlobalVariables.MainMenuRewardAdType == GlobalVariables.MainMenuRewardAdType_Shuffle)
                {
                    saveFile.shuffleJokerQuantity += shuffleReward;
                }
                else if (GlobalVariables.MainMenuRewardAdType == GlobalVariables.MainMenuRewardAdType_Undo)
                {
                    saveFile.undoJokerQuantity += undoReward;
                }

                saveFile.saveTime = DateTime.Now.ToString();
                generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
            }

            adMobControllers.GetComponent<RewardedAdController>().DestroyAd();
        }
    }

    void OnEnable()
    {
        if (GlobalVariables.MainMenuRewardAdType == GlobalVariables.MainMenuRewardAdType_Coin)
        {
            gameObject.GetComponent<Image>().sprite = CoinRewardImage;
            gameObject.GetComponentInChildren<TMP_Text>().text = coinReward.ToString();
        }
        else if (GlobalVariables.MainMenuRewardAdType == GlobalVariables.MainMenuRewardAdType_Shuffle)
        {
            gameObject.GetComponent<Image>().sprite = ShuffleRewardImage;
            gameObject.GetComponentInChildren<TMP_Text>().text = shuffleReward.ToString();
        }
        else if (GlobalVariables.MainMenuRewardAdType == GlobalVariables.MainMenuRewardAdType_Undo)
        {
            gameObject.GetComponent<Image>().sprite = UndoRewardImage;
            gameObject.GetComponentInChildren<TMP_Text>().text = undoReward.ToString();
        }
    }
}
