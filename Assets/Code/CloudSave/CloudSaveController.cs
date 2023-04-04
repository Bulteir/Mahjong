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
       

        /*
        //3 aþama 
        //1.servislerin aktif hale gelmesini bekliyoruz
        await UnityServices.InitializeAsync();
        //2.kullanýcýnýn login olmasýný bekliyoz
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        //istediðimiz verileri kaydediyoruz
        var data = new Dictionary<string, object> { { "MySaveKey", "HelloWorld" } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        */
    }

async public void LogInUnityCloudSaveServiceWithGooglePlay(string code)
    {
        try
        {
            Debug.Log("Süreç baþladý.");
            await UnityServices.InitializeAsync();

            // Check that scene has not been unloaded while processing async wait to prevent throw.
            if (this == null) return;

            //kullanýcý sign in olmadýysa
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                //await AuthenticationService.Instance.SignInAnonymouslyAsync();
                await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(code);
                if (this == null) return;
            }

            Debug.Log($"Player id: {AuthenticationService.Instance.PlayerId}");

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            if (this != null)
            {
                //artýk iþlem bitti herþeyi aktif yap manasýnda
                Debug.Log("baþarý ile bitti");

                var data = new Dictionary<string, object> { { "unlockedLevelCount", "99" } };
                await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            }
        }
    }

}
