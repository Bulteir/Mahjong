using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class AdMobController_MainMenu : MonoBehaviour
{
    #region UNITY MONOBEHAVIOR METHODS

    public void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);

        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

        //Add some test device IDs(replace with your own device IDs).
#if UNITY_IPHONE
        deviceIds.Add("DF229BBF2B1642998DFF3FFA52D9CD30");
#elif UNITY_ANDROID
        deviceIds.Add("d8680638de874f76b5395ce4a5f5ad66");
#endif
        // Configure TagForChildDirectedTreatment and test device IDs.

        RequestConfiguration requestConfiguration = new RequestConfiguration();
        requestConfiguration.TestDeviceIds = deviceIds;
        requestConfiguration.TagForChildDirectedTreatment = TagForChildDirectedTreatment.Unspecified;

        //RequestConfiguration requestConfiguration =
        //    new RequestConfiguration.Builder()
        //    .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
        //    .SetTestDeviceIds(deviceIds).build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);

        // Listen to application foreground / background events.
        //AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        Debug.Log("Initialization complete.");

        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // the main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            //statusText.text = "Initialization complete.";

            #region ana menüde gösterilen joker ve altýn kazandýran ödüllü reklamýn gösterilme sýklýðý ayarlamasý
            //oyun ilk defa kuruldjuðunda ödüllü reklam gösterim zamaný setlenir.
            string lastShowTime = PlayerPrefs.GetString("MainMenuRewardAdTime");
            if (lastShowTime == "")
            {
                PlayerPrefs.SetString("MainMenuRewardAdTime", DateTime.Now.ToLongTimeString());
                PlayerPrefs.SetInt("MainMenuRewardAdType", GlobalVariables.MainMenuRewardAdType);
                PlayerPrefs.Save();
                GetComponent<RewardedAdController>().LoadAd();
            }
            //daha önce setlenmiþ ödüllü reklam zamaný varsa 5 dakika geçmiþse tekrar setler.
            //Senaryo; oyuncu oynadý kapatttý 5 dakika sonra tekrar girdi buraya girer ya da 5 dkdan sonra bölüm içerisinden ana menüye döndü.
            else
            {
                string dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;

                DateTime showTime;
                DateTime.TryParseExact(lastShowTime, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out showTime);

                if ((DateTime.Now - showTime).TotalMinutes >= 5 || (DateTime.Now - showTime).TotalMinutes < 0)
                {
                    PlayerPrefs.SetString("MainMenuRewardAdTime", DateTime.Now.ToLongTimeString());

                    GlobalVariables.MainMenuRewardAdType = PlayerPrefs.GetInt("MainMenuRewardAdType");
                    if (GlobalVariables.MainMenuRewardAdType == GlobalVariables.MainMenuRewardAdType_Coin)
                    {
                        PlayerPrefs.SetInt("MainMenuRewardAdType", GlobalVariables.MainMenuRewardAdType_Shuffle);
                    }
                    else if (GlobalVariables.MainMenuRewardAdType == GlobalVariables.MainMenuRewardAdType_Shuffle)
                    {
                        PlayerPrefs.SetInt("MainMenuRewardAdType", GlobalVariables.MainMenuRewardAdType_Undo);

                    }
                    else if (GlobalVariables.MainMenuRewardAdType == GlobalVariables.MainMenuRewardAdType_Undo)
                    {
                        PlayerPrefs.SetInt("MainMenuRewardAdType", GlobalVariables.MainMenuRewardAdType_Coin);
                    }

                    PlayerPrefs.Save();
                    GetComponent<RewardedAdController>().LoadAd();
                }
            }
            #endregion

        });
    }

    #endregion

    public void OnAppStateChanged(AppState state)
    {
        // Display the app open ad when the app is foregrounded.
        UnityEngine.Debug.Log("App State is " + state);

        // OnAppStateChanged is not guaranteed to execute on the Unity UI thread.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (state == AppState.Foreground)
            {
                //ShowAppOpenAd();
            }
        });
    }
}
