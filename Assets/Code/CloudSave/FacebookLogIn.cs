using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using Unity.Services.Authentication;
using System.Linq;
using TMPro;

public class FacebookLogIn : MonoBehaviour
{
    public Button FacebookLogin_Btn;

    // Awake function from Unity's MonoBehavior
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...

            //kullan�c� birkez facebookla giri� yapt�ysa ba�lang��ta otomatik giri� yapar. Google i�in yapm�yoruz ��nk� zaten otomatik en ba�ta a��l�yor.
            if (PlayerPrefs.GetString("FacebookAutoLogin") == "true")
            {
                //burada google ile giri� yapt�ysa �zerine birde facebook giri�i i�in beklemesin. Bu y�zden androidde facebook authentication google ile giri�te s�k�nt� olursa yap�ls�n.
                FacebookLogin_Btn.interactable = false;
                Color PressTextColor = new Color(168.0f / 255, 145.0f / 255, 128.0f / 255);
                FacebookLogin_Btn.GetComponentInChildren<TMP_Text>().color = PressTextColor;
                Color PressButtonColor = new Color(115.0f / 255, 115.0f / 255, 115.0f / 255);
                FacebookLogin_Btn.image.color = PressButtonColor;

#if UNITY_iOS || UNITY_EDITOR
                //ios cihazlarda kullan�c� i�in �nceli�imiz game center ile giri�.
                if (PlayerPrefs.GetString("GameCenterAutoLogin") != "true")
                    LoginFacebook();
#endif
            }
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            GetComponent<CloudSaveController>().LogInUnityCloudSaveServiceWithFacebook(AccessToken.CurrentAccessToken.TokenString);
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    public void LoginFacebook()
    {
        FacebookLogin_Btn.interactable = false;
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    public void CheckLoginButtonStatus()
    {
        Color PressTextColor = new Color(168.0f / 255, 145.0f / 255, 128.0f / 255);
        Color PressButtonColor = new Color(115.0f / 255, 115.0f / 255, 115.0f / 255);

        if (GlobalVariables.internetAvaible == false)
        {
            FacebookLogin_Btn.interactable = false;
            FacebookLogin_Btn.GetComponentInChildren<TMP_Text>().color = Color.white;
            FacebookLogin_Btn.image.color = Color.white;
        }
        else
        {
            //hi�bir hesapla giri� yap�lmad�ysa
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                FacebookLogin_Btn.interactable = true;
                FacebookLogin_Btn.GetComponentInChildren<TMP_Text>().color = Color.white;
                FacebookLogin_Btn.image.color = Color.white;

            }//bir hesapla giri� yap�lm�� ve facebook hesab� ba�lanm��
            else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("facebook.com")).ToList().Count > 0)
            {
                FacebookLogin_Btn.interactable = false;
                FacebookLogin_Btn.GetComponentInChildren<TMP_Text>().color = PressTextColor;
                FacebookLogin_Btn.image.color = PressButtonColor;
            }
            else//bir hesapla giri� yap�lm�� ancak facebook hesab� ba�lanmam��.
            {
                FacebookLogin_Btn.interactable = true;
                FacebookLogin_Btn.GetComponentInChildren<TMP_Text>().color = Color.white;
                FacebookLogin_Btn.image.color = Color.white;
            }
        }
    }
}
