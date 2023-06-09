using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_GameOverMenu_MainMenu_Btn : MonoBehaviour
{
    public GameObject adMobControllers;

    public void OnClick()
    {
        GlobalVariables.intersitialAd_CallingObject = gameObject;
        adMobControllers.GetComponent<InterstitialAdController>().ShowAd();
    }

    public void MainMenu()
    {
        if (GlobalVariables.intersitialAd_CallingObject == gameObject)
        {
            GlobalVariables.intersitialAd_CallingObject = null;
            adMobControllers.GetComponent<InterstitialAdController>().DestroyAd();

            GlobalVariables.gameState = GlobalVariables.gameState_MainMenu;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
