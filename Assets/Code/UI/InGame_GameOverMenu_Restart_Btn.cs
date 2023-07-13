using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_GameOverMenu_Restart_Btn : MonoBehaviour
{
    public GameObject generalControllers;
    public GameObject adMobControllers;

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
            if (generalControllers.GetComponent<EnergyBarController>().IsThereEnoughEnergyForLevel())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            }
        }
        else
        {
            if (generalControllers.GetComponent<EnergyBarController>().IsThereEnoughEnergyForLevel())
            {
                GlobalVariables.intersitialAd_CallingObject = gameObject;
                if (adMobControllers.GetComponent<InterstitialAdController>().ShowAd() == false)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
                }
            }
        }
    }

    public void Restart()
    {
        if (GlobalVariables.intersitialAd_CallingObject == gameObject)
        {
            GlobalVariables.intersitialAd_CallingObject = null;
            adMobControllers.GetComponent<InterstitialAdController>().DestroyAd();
            adMobControllers.GetComponent<BannerViewController>().DestroyAd();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }
}
