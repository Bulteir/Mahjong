using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using System.Linq;
using Unity.Services.Core;
using TMPro;

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
//#if UNITY_ANDROID
    void Awake()
    {
        PlayGamesPlatform.Activate();
    }

    async void Start()
    {
        await LoginGooglePlayGamesStart();
    }

    //Google Play Game Servisine baðlanarak kullanýcýya özel token getirir.
    public Task LoginGooglePlayGamesStart()
    {
        var tcs = new TaskCompletionSource<object>();

        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            Debug.Log("Success:" + success.ToString());
            //kullanýcý google ile giriþ yapmayý birkez reddetmiþse yada herþey normalse ilk kez karþýlaþýlýyorsa
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
                //Daha önce facebook ile giriþ yapýlmýþsa. google ile giriþte herhangibir hata alýrsak facebook ile giriþ yapmayý dene.
                if (PlayerPrefs.GetString("FacebookAutoLogin") == "true")
                {
                    GetComponent<FacebookLogIn>().LoginFacebook();
                }
                else //anonim olarak giriþ yapmayý dene
                {
                    GetComponent<CloudSaveController>().LogInUnityCloudSaveServiceAnonymously();
                }

                Error = "Failed to retrieve Google play games authorization code";
                Debug.Log("Login Unsuccessful");
                tcs.SetResult("Login Unsuccessful");

            }
        });
        return tcs.Task;
    }

    public Task LoginGooglePlayGamesFromLoginButton()
    {
        var tcs = new TaskCompletionSource<object>();

        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            Debug.Log("Success:" + success.ToString());
            //kullanýcý google ile giriþ yapmayý birkez reddetmiþse yad aherþey normalse ilk kez karþýlaþýlýyorsa
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
            else if (success == SignInStatus.Canceled)
            {
 
                PlayGamesPlatform.Instance.ManuallyAuthenticate((success2) =>
                {
                    Debug.Log("Success2:" + success2.ToString());
                    if (success2 == SignInStatus.Success)
                    {
                        PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                        {
                            
                            Debug.Log("manuel authenticate sonrasý Authorization code: " + code);
                            Token = code;

                            //Unity Authentication Servisini çaðýrýyoruz. Google'dan aldýðýmýz kodla giriþ yapsýn diye.
                            GetComponent<CloudSaveController>().LogInUnityCloudSaveServiceWithGooglePlay(code);
                            // This token serves as an example to be used for SignInWithGooglePlayGames
                            tcs.SetResult(null);
                        });
                    }
                    else //anonim olarak giriþ yapmayý dene
                    {
                        GetComponent<CloudSaveController>().LogInUnityCloudSaveServiceAnonymously();
                    }
                });
            }
        });
        return tcs.Task;
    }

    public void ClickLoginButton()
    {
        GoogleLogin_Btn.interactable = false;
        PlayGamesPlatform.Activate();
        LoginGooglePlayGamesFromLoginButton();
    }

    public void CheckLoginButtonStatus()
    {
        Color PressTextColor = new Color(168.0f / 255, 145.0f / 255, 128.0f / 255);
        Color PressButtonColor = new Color(115.0f / 255, 115.0f / 255, 115.0f / 255);

        if (GlobalVariables.internetAvaible == false)
        {
            GoogleLogin_Btn.interactable = false;
            GoogleLogin_Btn.GetComponentInChildren<TMP_Text>().color = Color.white;
            GoogleLogin_Btn.image.color = Color.white;
        }
        else
        {
            //hiçbir hesapla giriþ yapýlmadýysa
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                GoogleLogin_Btn.interactable = true;
                GoogleLogin_Btn.GetComponentInChildren<TMP_Text>().color = Color.white;
                GoogleLogin_Btn.image.color = Color.white;

            }//bir hesapla giriþ yapýlmýþ ve google hesabý baðlanmýþ
            else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("google-play-games")).ToList().Count > 0)
            {
                GoogleLogin_Btn.interactable = false;
                GoogleLogin_Btn.GetComponentInChildren<TMP_Text>().color = PressTextColor;
                GoogleLogin_Btn.image.color = PressButtonColor;
            }
            else//bir hesapla giriþ yapýlmý ancak google hesabý baðlanmamýþ.
            {
                GoogleLogin_Btn.interactable = true;
                GoogleLogin_Btn.GetComponentInChildren<TMP_Text>().color = Color.white;
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
