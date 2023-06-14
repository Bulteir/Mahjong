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

            #region ana men�de g�sterilen joker ve alt�n kazand�ran �d�ll� reklam�n g�sterilme s�kl��� ayarlamas�
            //oyun ilk defa kuruldju�unda �d�ll� reklam g�sterim zaman� setlenir.
            string lastShowTime = PlayerPrefs.GetString("MainMenuRewardAdTime");
            if (lastShowTime == "")
            {
                PlayerPrefs.SetString("MainMenuRewardAdTime", DateTime.Now.ToLongTimeString());
                PlayerPrefs.SetInt("MainMenuRewardAdType", GlobalVariables.MainMenuRewardAdType);
                PlayerPrefs.Save();
                GetComponent<RewardedAdController>().LoadAd();
            }
            //daha �nce setlenmi� �d�ll� reklam zaman� varsa 5 dakika ge�mi�se tekrar setler.
            //Senaryo; oyuncu oynad� kapattt� 5 dakika sonra tekrar girdi buraya girer ya da 5 dkdan sonra b�l�m i�erisinden ana men�ye d�nd�.
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
