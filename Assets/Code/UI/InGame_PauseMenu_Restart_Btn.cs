using GoogleMobileAds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_PauseMenu_Restart_Btn : MonoBehaviour
{
    public GameObject adMobControllers;
    // Start is called before the first frame update
    public void OnClick()
    {
        GlobalVariables.intersitialAd_CallingObject = gameObject;
        adMobControllers.GetComponent<InterstitialAdController>().ShowAd();
    }

    public void Restart()
    {
        if (GlobalVariables.intersitialAd_CallingObject == gameObject)
        {
            GlobalVariables.intersitialAd_CallingObject = null;
            adMobControllers.GetComponent<InterstitialAdController>().DestroyAd();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }
}
