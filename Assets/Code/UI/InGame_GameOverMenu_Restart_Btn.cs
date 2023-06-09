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
        if (generalControllers.GetComponent<EnergyBarController>().IsThereEnoughEnergyForLevel())
        {
            GlobalVariables.intersitialAd_CallingObject = gameObject;
            adMobControllers.GetComponent<InterstitialAdController>().ShowAd();
        }
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
