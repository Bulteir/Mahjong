using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using System.Linq;

public class GooglePlayGameSignIn : MonoBehaviour
{
    public string Token;
    public string Error;
    public Button GoogleLogin_Btn;

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

    public void ClickLoginButton()
    {
        GoogleLogin_Btn.interactable = false;
        PlayGamesPlatform.Activate();
        LoginGooglePlayGames();
    }

    public void CheckLoginButtonStatus ()
    {
        if(GlobalVariables.internetAvaible == false)
        {
            GoogleLogin_Btn.interactable = false;
            GoogleLogin_Btn.image.color = Color.white;
        }
        else
        {
            //hiçbir hesapla giriþ yapýlmadýysa
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                GoogleLogin_Btn.interactable = true;
                GoogleLogin_Btn.image.color = Color.white;

            }//bir hesapla giriþ yapýlmýþ ve google hesabý baðlanmýþ
            else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("google-play-games")).ToList().Count > 0)
            {
                GoogleLogin_Btn.interactable = false;
                GoogleLogin_Btn.image.color = Color.blue;
            }
            else//bir hesapla giriþ yapýlmý ancak google hesabý baðlanmamýþ.
            {
                GoogleLogin_Btn.interactable = true;
                GoogleLogin_Btn.image.color = Color.white;
            }
        }
    }
}
