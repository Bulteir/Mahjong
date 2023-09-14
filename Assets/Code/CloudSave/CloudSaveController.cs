using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Threading.Tasks;
#if UNITY_ANDROID
using GooglePlayGames;
#endif
using TMPro;

public class CloudSaveController : MonoBehaviour
{
    public Button GoogleLogin_Btn;
    public Button FacebookLogin_Btn;
    public Button AppleGameCenterLogin_Btn;
    public GameObject LeaderBoardController;

    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();
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
                    Debug.Log("Google Play Game authentication kodu boþ geldi.");
                }
                else
                {

                    //kullanýcý sign in olmadýysa
                    if (!AuthenticationService.Instance.IsSignedIn)
                    {
                        Debug.Log("Unit cloud Authentication baþladý. Google code:" + code);
                        await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(code);
                        if (this == null) return;
                    }
                    else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("google-play-games")).ToList().Count == 0)//kullanýcýnýn hesabýna google baðlanmamýþtýr
                    {
                        Debug.Log("kullanýcýnýn hesabýna Google Play baðlanmamýþ. Google Play hesabý baðlanýyor. Google code:" + code);
                        await AuthenticationService.Instance.LinkWithGooglePlayGamesAsync(code);
                        if (this == null) return;

                    }

                    Debug.Log($"Player id: {AuthenticationService.Instance.PlayerId}");

                    GlobalVariables.cloudSaveSystemIsReady = true;

                    GoogleLogin_Btn.interactable = false;
                    Color PressTextColor = new Color(168.0f / 255, 145.0f / 255, 128.0f / 255);
                    GoogleLogin_Btn.GetComponentInChildren<TMP_Text>().color = PressTextColor;
                    Color PressButtonColor = new Color(115.0f / 255, 115.0f / 255, 115.0f / 255);
                    GoogleLogin_Btn.image.color = PressButtonColor;

                    GetComponent<GameSaveLoadController>().GameSaveDataSynchronization();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    async public void LogInUnityCloudSaveServiceWithFacebook(string code)
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
                    Debug.Log("Facebook authentication kodu boþ geldi.");
                }
                else
                {
                    //kullanýcý sign in olmadýysa
                    if (!AuthenticationService.Instance.IsSignedIn)
                    {
                        Debug.Log("Unit cloud Authentication baþladý. facebook code:" + code);
                        await AuthenticationService.Instance.SignInWithFacebookAsync(code);
                        if (this == null) return;
                    }
                    else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("facebook.com")).ToList().Count == 0)//kullanýcýnýn hesabýna facebook baðlanmamýþtýr
                    {
                        Debug.Log("kullanýcýnýn hesabýna Facebook baðlanmamýþ. Facebook hesabý baðlanýyor. Facebook code:" + code);

                        try
                        {
                            await AuthenticationService.Instance.LinkWithFacebookAsync(code);
                        }
                        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
                        {
                            Debug.LogError("Kullanýcnýn facebook hesabý baþka bir hesap ile iliþkilendirilmiþ.");

                            await AuthenticationService.Instance.DeleteAccountAsync();
                            await AuthenticationService.Instance.SignInWithFacebookAsync(code);
                        }
                    }

                    Debug.Log($"Player id: {AuthenticationService.Instance.PlayerId}");

                    GlobalVariables.cloudSaveSystemIsReady = true;

                    FacebookLogin_Btn.interactable = false;
                    Color PressTextColor = new Color(168.0f / 255, 145.0f / 255, 128.0f / 255);                    
                    FacebookLogin_Btn.GetComponentInChildren<TMP_Text>().color = PressTextColor;
                    Color PressButtonColor = new Color(115.0f / 255, 115.0f / 255, 115.0f / 255);
                    FacebookLogin_Btn.image.color = PressButtonColor;

                    PlayerPrefs.SetString("FacebookAutoLogin", "true");
                    PlayerPrefs.Save();
                    GetComponent<GameSaveLoadController>().GameSaveDataSynchronization();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    async public void LogInUnityCloudSaveServiceAnonymously()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Anonim olarak giriþ yapýldý. PlayerID:" + AuthenticationService.Instance.PlayerId);
        }
        else
        {
            Debug.Log("Unity servislerine giriþ yapýlmýþ. PlayerID:" + AuthenticationService.Instance.PlayerId);
        }
    }

    public async void SaveData(string value)
    {
        if (GlobalVariables.cloudSaveSystemIsReady)
        {
            //authentication sonrasý Unity cloud save servisine veri kaydetme örneði
            var data = new Dictionary<string, object> { { "save", value } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }
        else
        {
            Debug.Log("Unity Cloud Save sistemi Hazýr deðil. Veri kaydedilemedi.");
        }
    }

    public async Task<string> LoadData()
    {
        string json = "";
        if (GlobalVariables.cloudSaveSystemIsReady)
        {
            Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "save" });
            Debug.Log("bulut veri çekme:" + savedData.Count);
            if (savedData.Count > 0)
            {
                json = savedData["save"];
            }
        }
        else
        {
            Debug.Log("Unity Cloud Save sistemi Hazýr deðil. Veri getirilemedi.");
        }
        return json;
    }

    async public void LogInUnityCloudSaveServiceWithAppleGameCenter(string signature, string teamPlayerId, string publicKeyURL, string salt, ulong timestamp)
    {
        try
        {
            if (GlobalVariables.internetAvaible == false)
            {
                Debug.Log("Ýnternet baðlantýsý yok.");
            }
            else
            {
                if (signature == null || signature == "" || teamPlayerId == null || teamPlayerId == "" || publicKeyURL == null || publicKeyURL == "" || salt == null || salt == "" || timestamp.ToString() == null || timestamp.ToString() == "")
                {
                    Debug.Log("Apple game center authentication için gerekli parametreler boþ geldi.");
                }
                else
                {
                    if (!AuthenticationService.Instance.IsSignedIn) //kullanýcý sign in olmadýysa 
                    {
                        Debug.Log("Unit cloud Authentication baþladý. apple parametreler: signature:" + signature + "\nteamPlayerId:" + teamPlayerId + "\npublicKeyURL:" + publicKeyURL + "\nsalt:" + salt + "\ntimeStamp:" + timestamp);
                        await AuthenticationService.Instance.SignInWithAppleGameCenterAsync(signature, teamPlayerId, publicKeyURL, salt, timestamp);

                        if (this == null) return;
                    }
                    else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("apple-game-center")).ToList().Count == 0)//kullanýcýnýn hesabýna apple game center baðlanmamýþtýr
                    {
                        Debug.Log("kullanýcýnýn hesabýna Apple Game Center baðlanmamýþ. Game center hesabý baðlanýyor.");

                        try
                        {
                            await AuthenticationService.Instance.LinkWithAppleGameCenterAsync(signature, teamPlayerId, publicKeyURL, salt, timestamp);
                        }
                        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
                        {
                            Debug.LogError("Kullanýcnýn Game Center hesabý baþka bir hesap ile iliþkilendirilmiþ.");
                            await AuthenticationService.Instance.DeleteAccountAsync();
                            await AuthenticationService.Instance.SignInWithAppleGameCenterAsync(signature, teamPlayerId, publicKeyURL, salt, timestamp);
                        }
                    }

                    Debug.Log($"Player id: {AuthenticationService.Instance.PlayerId}");

                    GlobalVariables.cloudSaveSystemIsReady = true;

                    AppleGameCenterLogin_Btn.interactable = false;
                    Color PressTextColor = new Color(168.0f / 255, 145.0f / 255, 128.0f / 255);
                    AppleGameCenterLogin_Btn.GetComponentInChildren<TMP_Text>().color = PressTextColor;
                    Color PressButtonColor = new Color(115.0f / 255, 115.0f / 255, 115.0f / 255);
                    AppleGameCenterLogin_Btn.image.color = PressButtonColor;

                    PlayerPrefs.SetString("GameCenterAutoLogin", "true");
                    PlayerPrefs.Save();
                    GetComponent<GameSaveLoadController>().GameSaveDataSynchronization();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
