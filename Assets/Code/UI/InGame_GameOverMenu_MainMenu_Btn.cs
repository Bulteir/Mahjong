using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_GameOverMenu_MainMenu_Btn : MonoBehaviour
{
    public GameObject adMobControllers;
    public GameObject generalControllers;

    public void OnClick()
    {
        bool noAdsJokerActive = false;
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            if (saveFile.noAdsJokerActive == true)//kullanýcý no ads eþyasý alýnmýþsa.
            {
                noAdsJokerActive = true;
            }
        }

        if (noAdsJokerActive)
        {
            GlobalVariables.gameState = GlobalVariables.gameState_MenuBackground;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        else
        {
            GlobalVariables.intersitialAd_CallingObject = gameObject;
            adMobControllers.GetComponent<InterstitialAdController>().ShowAd();
        }
    }

    public void MainMenu()
    {
        if (GlobalVariables.intersitialAd_CallingObject == gameObject)
        {
            GlobalVariables.intersitialAd_CallingObject = null;
            adMobControllers.GetComponent<InterstitialAdController>().DestroyAd();
            adMobControllers.GetComponent<BannerViewController>().DestroyAd();

            GlobalVariables.gameState = GlobalVariables.gameState_MenuBackground;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
