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
                Debug.Log("Ýnternet baðlantýsý yok.");
            }
            else
            {
                if (code == null || code == "")
                {
                    Debug.Log("Google Play Game autjentication kodu boþ geldi.");
                }
                else
                {
                    //unity servislerini baþlatýr. Ancak biz google vb. ile giriþ yapacaðýmýz için o tarafta baþlatýlýyor.

                    await UnityServices.InitializeAsync();

                    // Check that scene has not been unloaded while processing async wait to prevent throw.
                    if (this == null) return;


                    //kullanýcý sign in olmadýysa
                    if (!AuthenticationService.Instance.IsSignedIn)
                    {
                        //anonim olarak giriþ yapmayý saðlar ancak uygulama silinip yüklendiðinde kullanýcýnýn kodu deðiþeceði için save dosyasýný kaybeder.
                        //await AuthenticationService.Instance.SignInAnonymouslyAsync();
                        Debug.Log("Unit cloud Authentication baþladý. google code:" + code);
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
            //authentication sonrasý Unity cloud save servisine veri kaydetme örneði
            var data = new Dictionary<string, object> { { "unlockedLevelCount", "99.8" } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }
        else
        {
            Debug.Log("Unity Cloud Save sistemi Hazýr deðil. Veri kaydedilemedi.");
        }

    }
}
