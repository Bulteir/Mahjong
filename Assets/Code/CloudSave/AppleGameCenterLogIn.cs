using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Apple.GameKit;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class AppleGameCenterLogIn : MonoBehaviour
{
    string Signature;
    string TeamPlayerID;
    string Salt;
    string PublicKeyUrl;
    string Timestamp;

    bool buttonClickFlag = false;

    public Button AppleGameCenterLogin_Btn;

    // Start is called before the first frame update
    async void Start()
    {
        if (PlayerPrefs.GetString("GameCenterAutoLogin") == "true")
        {
            await Login();
        }
    }

    async void Update()
    {
        if (buttonClickFlag)
        {
            buttonClickFlag = false;

            await Login();
        }
    }

    public async Task Login()
    {
        try
        {
            //apple game center ile giri� yap�lmam��sa, game center ile giri� yapmay� dener.
            if (!GKLocalPlayer.Local.IsAuthenticated)
            {
                // Perform the authentication.
                var player = await GKLocalPlayer.Authenticate();
                Debug.Log($"GameKit Authentication: player {player}");

                // Grab the display name.
                var localPlayer = GKLocalPlayer.Local;
                Debug.Log($"Local Player: {localPlayer.DisplayName}");

                // Fetch the items.
                var fetchItemsResponse = await GKLocalPlayer.Local.FetchItems();

                Signature = Convert.ToBase64String(fetchItemsResponse.Signature);
                TeamPlayerID = localPlayer.TeamPlayerId;
                Debug.Log($"Team Player ID: {TeamPlayerID}");

                Salt = Convert.ToBase64String(fetchItemsResponse.Salt);
                PublicKeyUrl = fetchItemsResponse.PublicKeyUrl;
                Timestamp = fetchItemsResponse.Timestamp.ToString();

                Debug.Log($"GameKit Authentication: signature => {Signature}");
                Debug.Log($"GameKit Authentication: publickeyurl => {PublicKeyUrl}");
                Debug.Log($"GameKit Authentication: salt => {Salt}");
                Debug.Log($"GameKit Authentication: Timestamp => {Timestamp}");

                AppleGameCenterLogin_Btn.interactable = false;
                Color PressTextColor = new Color(168.0f / 255, 145.0f / 255, 128.0f / 255);
                AppleGameCenterLogin_Btn.GetComponentInChildren<TMP_Text>().color = PressTextColor;
                Color PressButtonColor = new Color(115.0f / 255, 115.0f / 255, 115.0f / 255);
                AppleGameCenterLogin_Btn.image.color = PressButtonColor;

                GetComponent<CloudSaveController>().LogInUnityCloudSaveServiceWithAppleGameCenter(Signature, TeamPlayerID, PublicKeyUrl, Salt, ulong.Parse(Timestamp));
            }
            else
            {
                Debug.Log("AppleGameCenter player already logged in.");

                AppleGameCenterLogin_Btn.interactable = false;
                Color PressTextColor = new Color(168.0f / 255, 145.0f / 255, 128.0f / 255);
                AppleGameCenterLogin_Btn.GetComponentInChildren<TMP_Text>().color = PressTextColor;
                Color PressButtonColor = new Color(115.0f / 255, 115.0f / 255, 115.0f / 255);
                AppleGameCenterLogin_Btn.image.color = PressButtonColor;
            }
        }
        catch //apple ile giri� yapmada bir hata al�n�rsa 
        {
            //Daha �nce facebook ile giri� yap�lm��sa. apple game center ile giri�te herhangibir hata al�rsak facebook ile giri� yapmay� dene.
            if (PlayerPrefs.GetString("FacebookAutoLogin") == "true")
            {
                GetComponent<FacebookLogIn>().LoginFacebook();
            }
            else //anonim olarak giri� yapmay� dene
            {
                GetComponent<CloudSaveController>().LogInUnityCloudSaveServiceAnonymously();
            }
        }
    }

    public void StartAppleGameCenterLogIn()
    {
        buttonClickFlag = true;
    }

    public void CheckLoginButtonStatus()
    {
        Color PressTextColor = new Color(168.0f / 255, 145.0f / 255, 128.0f / 255);
        Color PressButtonColor = new Color(115.0f / 255, 115.0f / 255, 115.0f / 255);

        if (GlobalVariables.internetAvaible == false)
        {
            AppleGameCenterLogin_Btn.interactable = false;
            AppleGameCenterLogin_Btn.GetComponentInChildren<TMP_Text>().color = Color.white;
            AppleGameCenterLogin_Btn.image.color = Color.white;
        }
        else
        {
            //hi�bir hesapla giri� yap�lmad�ysa
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                AppleGameCenterLogin_Btn.interactable = true;
                AppleGameCenterLogin_Btn.GetComponentInChildren<TMP_Text>().color = Color.white;
                AppleGameCenterLogin_Btn.image.color = Color.white;

            }//bir hesapla giri� yap�lm�� ve game center hesab� ba�lanm��
            else if (AuthenticationService.Instance.PlayerInfo.Identities.Where(i => i.TypeId.Contains("apple-game-center")).ToList().Count > 0)
            {
                AppleGameCenterLogin_Btn.interactable = false;
                AppleGameCenterLogin_Btn.GetComponentInChildren<TMP_Text>().color = PressTextColor;
                AppleGameCenterLogin_Btn.image.color = PressButtonColor;
            }
            else//bir hesapla giri� yap�lm�� ancak game center hesab� ba�lanmam��.
            {
                AppleGameCenterLogin_Btn.interactable = true;
                AppleGameCenterLogin_Btn.GetComponentInChildren<TMP_Text>().color = Color.white;
                AppleGameCenterLogin_Btn.image.color = Color.white;
            }
        }
    }

}
