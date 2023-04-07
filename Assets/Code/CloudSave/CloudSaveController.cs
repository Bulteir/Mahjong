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
                Debug.Log("�nternet ba�lant�s� yok.");
            }
            else
            {
                if (code == null || code == "")
                {
                    Debug.Log("Google Play Game authentication kodu bo� geldi.");
                }
                else
                {

                    await UnityServices.InitializeAsync();
                    if (this == null) return;

                    //kullan�c� sign in olmad�ysa
                    if (!AuthenticationService.Instance.IsSignedIn)
                    {
                        Debug.Log("Unit cloud Authentication ba�lad�. Google code:" + code);
                        await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(code);
                        if (this == null) return;
                    }
                    else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("google-play-games")).ToList().Count == 0)//kullan�c�n�n hesab�na google ba�lanmam��t�r
                    {
                        Debug.Log("kullan�c�n�n hesab�na Google Play ba�lanmam��. Google Play hesab� ba�lan�yor. Google code:" + code);
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
                Debug.Log("�nternet ba�lant�s� yok.");
            }
            else
            {
                if (code == null || code == "")
                {
                    Debug.Log("Facebook authentication kodu bo� geldi.");
                }
                else
                {

                    await UnityServices.InitializeAsync();
                    if (this == null) return;

                    //kullan�c� sign in olmad�ysa
                    if (!AuthenticationService.Instance.IsSignedIn)
                    {
                        Debug.Log("Unit cloud Authentication ba�lad�. facebook code:" + code);
                        await AuthenticationService.Instance.SignInWithFacebookAsync(code);
                        if (this == null) return;
                    }
                    else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("facebook.com")).ToList().Count == 0)//kullan�n�n hesab�na facebook ka�lanmam��t�r
                    {
                        Debug.Log("kullan�c�n�n hesab�na Facebook ba�lanmam��. Facebook hesab� ba�lan�yor. Facebook code:" + code);
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
            //authentication sonras� Unity cloud save servisine veri kaydetme �rne�i
            var data = new Dictionary<string, object> { { "unlockedLevelCount", value } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }
        else
        {
            Debug.Log("Unity Cloud Save sistemi Haz�r de�il. Veri kaydedilemedi.");
        }

    }
}
