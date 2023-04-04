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
        await UnityServices.InitializeAsync();
        await LoginGooglePlayGames();
        await SignInWithGooglePlayGamesAsync(Token);
    }

    //Fetch the Token / Auth code
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

                    //GetComponent<CloudSaveController>().LogInUnityCloudSaveServiceWithGooglePlay(code);
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


    async Task SignInWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            Debug.Log("Unit cloud Authentication baþladý. google code:" + authCode);
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); //Display the Unity Authentication PlayerID
            Debug.Log("SignIn is successful.");

            var data = new Dictionary<string, object> { { "unlockedLevelCount", "70" } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    //void Awake()
    //{
    //    //Initialize PlayGamesPlatform
    //    PlayGamesPlatform.Activate();
    //    LoginGooglePlayGames();
    //}

    //public void LoginGooglePlayGames()
    //{
    //    PlayGamesPlatform.Instance.Authenticate((success) =>
    //    {
    //        if (success == SignInStatus.Success)
    //        {
    //            Debug.Log("Login with Google Play games successful.");

    //            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
    //            {
    //                Debug.Log("Authorization code: " + code);
    //                Token = code;
    //                // This token serves as an example to be used for SignInWithGooglePlayGames
    //            });
    //        }
    //        else
    //        {
    //            Error = "Failed to retrieve Google play games authorization code";
    //            Debug.Log("Login Unsuccessful");
    //        }
    //    });
    //}
}
