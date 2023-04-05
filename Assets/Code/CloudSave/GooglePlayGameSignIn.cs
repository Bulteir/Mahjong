using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using System.Collections.Generic;
using Unity.Services.CloudSave;

public class GooglePlayGameSignIn : MonoBehaviour
{
    public string Token;
    public string Error;

    void Awake()
    {
        PlayGamesPlatform.Activate();
    }

    async void Start()
    {
        await LoginGooglePlayGames();
    }

    //Google Play Game Servisine baðlanarak kullanýcýya özel token getirir.
    public Task LoginGooglePlayGames()
    {
        var tcs = new TaskCompletionSource<object>();
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;

                    //Unity Authentication Servisini çaðýrýyoruz. Google'dan aldýðýmýz kodla giriþ yapsýn diye.
                    GetComponent<CloudSaveController>().LogInUnityCloudSaveServiceWithGooglePlay(code);
                    // This token serves as an example to be used for SignInWithGooglePlayGames
                    tcs.SetResult(null);
                });
            }
            else
            {
                Error = "Failed to retrieve Google play games authorization code";
                Debug.Log("Login Unsuccessful");
                tcs.SetException(new Exception("Failed"));
            }
        });
        return tcs.Task;
    }
}
