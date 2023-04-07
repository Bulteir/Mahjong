using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System;
using System.Linq;
using UnityEngine.UI;

public class CloudSaveController : MonoBehaviour
{
    public Button GoogleLogin_Btn;
    public Button FacebookLogin_Btn;
    // Start is called before the first frame update
    void Start()
    {

    }

    async public void LogInUnityCloudSaveServiceWithGooglePlay(string code)
    {
        try
        {
            if (GlobalVariables.internetAvaible == false)
            {
                Debug.Log("Ýnternet baðlantýsý yok.");
            }
            else
            {
                if (code == null || code == "")
                {
                    Debug.Log("Google Play Game authentication kodu boþ geldi.");
                }
                else
                {

                    await UnityServices.InitializeAsync();
                    if (this == null) return;

                    //kullanýcý sign in olmadýysa
                    if (!AuthenticationService.Instance.IsSignedIn)
                    {
                        Debug.Log("Unit cloud Authentication baþladý. Google code:" + code);
                        await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(code);
                        if (this == null) return;
                    }
                    else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("google-play-games")).ToList().Count == 0)//kullanýcýnýn hesabýna google baðlanmamýþtýr
                    {
                        Debug.Log("kullanýcýnýn hesabýna Google Play baðlanmamýþ. Google Play hesabý baðlanýyor. Google code:" + code);
                        await AuthenticationService.Instance.LinkWithGoogleAsync(code);
                        if (this == null) return;
                    }

                    Debug.Log($"Player id: {AuthenticationService.Instance.PlayerId}");

                    GlobalVariables.cloudSaveSystemIsReady = true;

                    GoogleLogin_Btn.interactable = false;
                    GoogleLogin_Btn.image.color = Color.blue;

                    //SaveData("google");

                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    async public void LogInUnityCloudSaveServiceWithFacebook(string code)
    {
        try
        {
            if (GlobalVariables.internetAvaible == false)
            {
                Debug.Log("Ýnternet baðlantýsý yok.");
            }
            else
            {
                if (code == null || code == "")
                {
                    Debug.Log("Facebook authentication kodu boþ geldi.");
                }
                else
                {

                    await UnityServices.InitializeAsync();
                    if (this == null) return;

                    //kullanýcý sign in olmadýysa
                    if (!AuthenticationService.Instance.IsSignedIn)
                    {
                        Debug.Log("Unit cloud Authentication baþladý. facebook code:" + code);
                        await AuthenticationService.Instance.SignInWithFacebookAsync(code);
                        if (this == null) return;
                    }
                    else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("facebook.com")).ToList().Count == 0)//kullanýnýn hesabýna facebook kaðlanmamýþtýr
                    {
                        Debug.Log("kullanýcýnýn hesabýna Facebook baðlanmamýþ. Facebook hesabý baðlanýyor. Facebook code:" + code);
                        await AuthenticationService.Instance.LinkWithFacebookAsync(code);
                        if (this == null) return;
                    }

                    Debug.Log($"Player id: {AuthenticationService.Instance.PlayerId}");

                    GlobalVariables.cloudSaveSystemIsReady = true;

                    FacebookLogin_Btn.interactable = false;
                    FacebookLogin_Btn.image.color = Color.blue;

                    PlayerPrefs.SetString("FacebookAutoLogin", "true");
                    PlayerPrefs.Save();
                    //SaveData("facebook");

                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    async void SaveData(string value)
    {
        if (GlobalVariables.cloudSaveSystemIsReady)
        {
            //authentication sonrasý Unity cloud save servisine veri kaydetme örneði
            var data = new Dictionary<string, object> { { "unlockedLevelCount", value } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }
        else
        {
            Debug.Log("Unity Cloud Save sistemi Hazýr deðil. Veri kaydedilemedi.");
        }

    }
}
