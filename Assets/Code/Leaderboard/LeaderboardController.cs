using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using GooglePlayGames;
using Facebook.Unity;

public class LeaderboardController : MonoBehaviour
{
    const string LeaderboardId = "BestTimes";
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; }
    List<string> FriendIds { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        //editörde test yapmak için
        //if (UnityServices.State != ServicesInitializationState.Initializing || UnityServices.State != ServicesInitializationState.Initialized)
        //{
        //    await UnityServices.InitializeAsync();
        //}

        //if (AuthenticationService.Instance.IsAuthorized == false)
        //{
        //    await SignInAnonymously();
        //}

    }

    async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void SetPlayerName()
    {
        string playerName = "";
        //google ile giriþ yapmýþtýr
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            playerName = PlayGamesPlatform.Instance.GetUserDisplayName();
            Debug.Log("Google ile giriþ yapýlmýþ. Oyuncu ismi:" + playerName);
        }
        else if(FB.IsLoggedIn) //facebook ile giriþ yapmýþsa
        {
            playerName = FB.Mobile.CurrentProfile().Name;
            Debug.Log("Facebook ile giriþ yapýlmýþ. Oyuncu ismi:" + playerName);
        }

        playerName = playerName.Replace(" ", "_");
        Debug.Log("Oyuncu adý güncelleniyor");
        await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
    }

    public async void SetFalsePlayerName()
    {
        string playerName = "Deneme";
        Debug.Log("Oyuncu adý güncelleniyor");
        await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
    }

    public async void AddScore()
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, 105);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetScores()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPaginatedScores()
    {
        Offset = 10;
        Limit = 10;
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions { Offset = Offset, Limit = Limit });
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    //giriþ yapan oyuncunun bilgilerini getir
    public async void GetPlayerScore()
    {
        var scoreResponse =
            await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    //oyuncunun X üstü ile X altýndaki oyuncularý getirir
    public async void GetPlayerRange()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetPlayerRangeAsync(LeaderboardId, new GetPlayerRangeOptions { RangeLimit = 1 });
        //var scoresResponse = await LeaderboardsService.Instance.GetPlayerRangeAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }
}
