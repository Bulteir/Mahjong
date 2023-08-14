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
                Debug.Log("�nternet ba�lant�s� yok.");
            }
            else
            {
                if (code == null || code == "")
                {
                    Debug.Log("Google Play Game authentication kodu bo� geldi.");
                }
                else
                {

                    //kullan�c� sign in olmad�ysa
                    if (!AuthenticationService.Instance.IsSignedIn)
                    {
                        Debug.Log("Unit cloud Authentication ba�lad�. Google code:" + code);
                        await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(code);
                        if (this == null) return;
                    }
                    else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("google-play-games")).ToList().Count == 0)//kullan�c�n�n hesab�na google ba�lanmam��t�r
                    {
                        Debug.Log("kullan�c�n�n hesab�na Google Play ba�lanmam��. Google Play hesab� ba�lan�yor. Google code:" + code);
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
                    LeaderBoardController.GetComponent<LeaderboardController>().SetPlayerName();
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
                Debug.Log("�nternet ba�lant�s� yok.");
            }
            else
            {
                if (code == null || code == "")
                {
                    Debug.Log("Facebook authentication kodu bo� geldi.");
                }
                else
                {
                    //kullan�c� sign in olmad�ysa
                    if (!AuthenticationService.Instance.IsSignedIn)
                    {
                        Debug.Log("Unit cloud Authentication ba�lad�. facebook code:" + code);
                        await AuthenticationService.Instance.SignInWithFacebookAsync(code);
                        if (this == null) return;
                    }
                    else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("facebook.com")).ToList().Count == 0)//kullan�c�n�n hesab�na facebook ba�lanmam��t�r
                    {
                        Debug.Log("kullan�c�n�n hesab�na Facebook ba�lanmam��. Facebook hesab� ba�lan�yor. Facebook code:" + code);

                        try
                        {
                            await AuthenticationService.Instance.LinkWithFacebookAsync(code);
                        }
                        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
                        {
                            Debug.LogError("Kullan�cn�n facebook hesab� ba�ka bir hesap ile ili�kilendirilmi�.");

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
                    LeaderBoardController.GetComponent<LeaderboardController>().SetPlayerName();
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
            Debug.Log("Anonim olarak giri� yap�ld�. PlayerID:" + AuthenticationService.Instance.PlayerId);
        }
        else
        {
            Debug.Log("Unity servislerine giri� yap�lm��. PlayerID:" + AuthenticationService.Instance.PlayerId);
        }
    }

    public async void SaveData(string value)
    {
        if (GlobalVariables.cloudSaveSystemIsReady)
        {
            //authentication sonras� Unity cloud save servisine veri kaydetme �rne�i
            var data = new Dictionary<string, object> { { "save", value } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }
        else
        {
            Debug.Log("Unity Cloud Save sistemi Haz�r de�il. Veri kaydedilemedi.");
        }
    }

    public async Task<string> LoadData()
    {
        string json = "";
        if (GlobalVariables.cloudSaveSystemIsReady)
        {
            Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "save" });
            Debug.Log("bulut veri �ekme:" + savedData.Count);
            if (savedData.Count > 0)
            {
                json = savedData["save"];
            }
        }
        else
        {
            Debug.Log("Unity Cloud Save sistemi Haz�r de�il. Veri getirilemedi.");
        }
        return json;
    }
}
