using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System;

public class CloudSaveController : MonoBehaviour
{
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
                    Debug.Log("Google Play Game autjentication kodu bo� geldi.");
                }
                else
                {
                    //unity servislerini ba�lat�r. Ancak biz google vb. ile giri� yapaca��m�z i�in o tarafta ba�lat�l�yor.

                    await UnityServices.InitializeAsync();

                    // Check that scene has not been unloaded while processing async wait to prevent throw.
                    if (this == null) return;


                    //kullan�c� sign in olmad�ysa
                    if (!AuthenticationService.Instance.IsSignedIn)
                    {
                        //anonim olarak giri� yapmay� sa�lar ancak uygulama silinip y�klendi�inde kullan�c�n�n kodu de�i�ece�i i�in save dosyas�n� kaybeder.
                        //await AuthenticationService.Instance.SignInAnonymouslyAsync();
                        Debug.Log("Unit cloud Authentication ba�lad�. google code:" + code);
                        await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(code);
                        if (this == null) return;
                    }
                    Debug.Log($"Player id: {AuthenticationService.Instance.PlayerId}");

                    GlobalVariables.cloudSaveSystemIsReady = true;

                    SaveData();

                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    async void SaveData()
    {
        if (GlobalVariables.cloudSaveSystemIsReady)
        {
            //authentication sonras� Unity cloud save servisine veri kaydetme �rne�i
            var data = new Dictionary<string, object> { { "unlockedLevelCount", "99.8" } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }
        else
        {
            Debug.Log("Unity Cloud Save sistemi Haz�r de�il. Veri kaydedilemedi.");
        }

    }
}
