using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdMobControllers : MonoBehaviour
{
    public GameObject generalControllers;

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

            SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
            if (saveFile.saveTime != null)//Kay�tl� save dosyas� varsa
            {
                if (saveFile.noAdsJokerActive == false)//kullan�c� no ads e�yas� almam��t�r.
                {
                    GetComponent<BannerViewController>().LoadAd();
                    GetComponent<InterstitialAdController>().LoadAd();
                }
            }
            else //save dosyas� yok. Default olarak banner ve ge�i� reklam� y�kle
            {
                GetComponent<BannerViewController>().LoadAd();
                GetComponent<InterstitialAdController>().LoadAd();
            }

            GetComponent<RewardedAdController>().LoadAd();
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
