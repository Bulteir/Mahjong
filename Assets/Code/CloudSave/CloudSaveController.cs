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
        //3 a�ama 
        //1.servislerin aktif hale gelmesini bekliyoruz
        await UnityServices.InitializeAsync();
        //2.kullan�c�n�n login olmas�n� bekliyoz
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        //istedi�imiz verileri kaydediyoruz
        var data = new Dictionary<string, object> { { "MySaveKey", "HelloWorld" } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        */
    }

async public void LogInUnityCloudSaveServiceWithGooglePlay(string code)
    {
        try
        {
            Debug.Log("S�re� ba�lad�.");
            await UnityServices.InitializeAsync();

            // Check that scene has not been unloaded while processing async wait to prevent throw.
            if (this == null) return;

            //kullan�c� sign in olmad�ysa
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
                //art�k i�lem bitti her�eyi aktif yap manas�nda
                Debug.Log("ba�ar� ile bitti");

                var data = new Dictionary<string, object> { { "unlockedLevelCount", "99" } };
                await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            }
        }
    }

}
