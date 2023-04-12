using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using System.Linq;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class GooglePlayGameSignIn : MonoBehaviour
{
    public string Token;
    public string Error;
    public Button GoogleLogin_Btn;

#if UNITY_ANDROID && !UNITY_EDITOR
    void Awake()
    {
        PlayGamesPlatform.Activate();
    }

    async void Start()
    {
        await LoginGooglePlayGames();
    }

    //Google Play Game Servisine ba�lanarak kullan�c�ya �zel token getirir.
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

                    //Unity Authentication Servisini �a��r�yoruz. Google'dan ald���m�z kodla giri� yaps�n diye.
                    GetComponent<CloudSaveController>().LogInUnityCloudSaveServiceWithGooglePlay(code);
                    // This token serves as an example to be used for SignInWithGooglePlayGames
                    tcs.SetResult(null);
                });
            }
            else
            {
                //Daha �nce facebook ile giri� yap�lm��sa. google ile giri�te herhangibir hata al�rsak facebook ile giri� yapmay� dene.
                if (PlayerPrefs.GetString("FacebookAutoLogin") == "true")
                {   
                    GetComponent<FacebookLogIn>().LoginFacebook();
                }
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
            //hi�bir hesapla giri� yap�lmad�ysa
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                GoogleLogin_Btn.interactable = true;
                GoogleLogin_Btn.image.color = Color.white;

            }//bir hesapla giri� yap�lm�� ve google hesab� ba�lanm��
            else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("google-play-games")).ToList().Count > 0)
            {
                GoogleLogin_Btn.interactable = false;
                GoogleLogin_Btn.image.color = Color.blue;
            }
            else//bir hesapla giri� yap�lm� ancak google hesab� ba�lanmam��.
            {
                GoogleLogin_Btn.interactable = true;
                GoogleLogin_Btn.image.color = Color.white;
            }
        }
    }
#elif UNITY_EDITOR
    public void CheckLoginButtonStatus()
    {
    }
#endif
}
