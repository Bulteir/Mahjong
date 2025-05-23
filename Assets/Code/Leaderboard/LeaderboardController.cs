using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
#if UNITY_ANDROID
using GooglePlayGames;
#endif
using Facebook.Unity;
using Unity.Services.Leaderboards.Models;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using Unity.VisualScripting;
using System;

public class LeaderboardController : MonoBehaviour
{
    public GameObject TotalRecordText;
    public GameObject TemplateRow;
    public GameObject TemplateBrace;
    public GameObject ScrollviewContent;
    public GameObject LoadAnimation;
    public GameObject RowParent;

    public int LineSpacing;
    int LastRowIndex;
    float nextPosY = 0;
    float rowGroupSpacing = 0;

    //Start is called before the first frame update
    void Start()
    {
        //a�a��daki metadu telefonlarda kulland���m�zda listeleme s�ras�nda bug olu�uyor. Sat�rlar birden fazla kez yaz�l�yor, hatal� ki�i highlight ediliyor vs. 
        //Ancak edit�rde test ama�l� kullan�labilir.

        //async void Start()
        //{
        //edit�rde test yapmak i�in
        //if (UnityServices.State != ServicesInitializationState.Initializing || UnityServices.State != ServicesInitializationState.Initialized)
        //{
        //    await UnityServices.InitializeAsync();
        //}

        ////AuthenticationService.Instance.ClearSessionToken();
        //if (AuthenticationService.Instance.IsAuthorized == false)
        //{
        //    await SignInAnonymously();
        //}
        //await AuthenticationService.Instance.UpdatePlayerNameAsync("Talha");

        //FillLeaderboardList();
    }

    //edit�rde test etmek i�in
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

    public async void SetPlayerName(string name)
    {
        string playerName = name;

        playerName = playerName.Replace(" ", "_");
        Debug.Log("Oyuncu ad� g�ncelleniyor");
        
        //hi�bir�ey ile giri� yapmad�ysa veya ba�ka bir sebeple bo� kullan�c� ad� setlememek ad�na
        if (playerName.Length > 1 && playerName.Length < 50)
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
        }
    }

    public async void SetFalsePlayerName()
    {
        string playerName = "Deneme";
        Debug.Log("Oyuncu ad� g�ncelleniyor");
        await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
    }

    public async void AddScore(int score)
    {
        if (AuthenticationService.Instance.IsAuthorized == false)
        {
            await SignInAnonymously();
        }

        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(GlobalVariables.LeaderboardId_Richest, score);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        //Ana men�deki leaderboard butonuna t�kland���nda hemen kullan�c�n�n totalcoini addscore ile unity'e g�nderileyor. ve ard�ndan leaderboard ekran� a��l�yor. Addscore i�lemi bitince listelemeye ge�iliyor.
        //kullan�c�n�n puan� ve higlight edilen ki�i'de s�k�nt� olmamas� ad�na score eklenmesi bittikten sonra listeyi dolduruyoruz.
        FillLeaderboardList();
    }

    public async void AddScoreTestButton()
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(GlobalVariables.LeaderboardId_Richest, 900);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetScores()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(GlobalVariables.LeaderboardId_Richest);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPaginatedScores()
    {
        int Offset = 10;
        int Limit = 10;
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(GlobalVariables.LeaderboardId_Richest, new GetScoresOptions { Offset = Offset, Limit = Limit });
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    //giri� yapan oyuncunun bilgilerini getir
    public async void GetPlayerScore()
    {
        var scoreResponse =
            await LeaderboardsService.Instance.GetPlayerScoreAsync(GlobalVariables.LeaderboardId_Richest);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    //oyuncunun X �st� ile X alt�ndaki oyuncular� getirir
    public async void GetPlayerRange()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetPlayerRangeAsync(GlobalVariables.LeaderboardId_Richest, new GetPlayerRangeOptions { RangeLimit = 1 });
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void FillLeaderboardList()
    {
        try
        {
            if (GlobalVariables.internetAvaible)
            {
                //daha �nceden olu�turulmu� sat�rlar varsa sil
                foreach (Transform oldRows in RowParent.transform)
                {
                    Destroy(oldRows.gameObject);
                }

                LastRowIndex = 0;
                nextPosY = 0;
                rowGroupSpacing = 0;

                LoadAnimation.GetComponent<LoadSaveAnimationController>().StartAnimation(LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Loading"));
                LeaderboardEntry scoreResponse = null;

                //kullan�c�n�n kay�tl� puan� varm� kontrol ediyoruz. Puan� varsa s�ralamas�na g�re farkl� �ekilde g�sterim yap�yoruz.
                //Kullan�c�n�n kay�tl� puan� yoksa hata d�n�yor. Hata ald���m�zda kullan�c�n skoru yok diye kabul edebiliriz.
                try
                {
                    scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(GlobalVariables.LeaderboardId_Richest);
                }
                catch (Exception)
                {

                }

                //Oyuncunun kay�tl� puan� yoktur ya da daha k���k ihtimalle ele almad���m�z ba�ka bir hata olu�mu�tur.
                //bu durumda ilk 30 kayd� g�ster
                if (scoreResponse == null)
                {

                    int Limit = 30;
                    int page = 0;
                    int Offset = Limit * page;

                    LeaderboardScoresPage topScores = await LeaderboardsService.Instance.GetScoresAsync(GlobalVariables.LeaderboardId_Richest, new GetScoresOptions { Offset = Offset, Limit = Limit });

                    for (int i = 0; i < topScores.Results.Count; i++)
                    {
                        FillLeaderboardRow(i, topScores.Results[i], new Color(9f / 255f, 183f / 255f, 255f / 255f, 0f / 255f));
                    }

                    float newBraceSpacing = 0;
                    //E�er toplam kay�t say�s� listelenenden fazla ise bir ayra� at�yoruz.
                    if (topScores.Total > Limit)
                    {
                        GameObject newBrace = Instantiate(TemplateBrace, RowParent.transform);
                        newBrace.SetActive(true);
                        newBrace.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (topScores.Results.Count * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing)) + LineSpacing);
                        newBraceSpacing = newBrace.GetComponent<RectTransform>().sizeDelta.y - LineSpacing;
                    }

                    //toplam kay�t say�s� Text'i
                    TotalRecordText.SetActive(true);
                    TotalRecordText.GetComponent<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Total") + ": " + topScores.Total.ToString();
                    TotalRecordText.GetComponent<RectTransform>().anchoredPosition = new Vector2(TotalRecordText.GetComponent<RectTransform>().anchoredPosition.x, (topScores.Results.Count * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing)) - newBraceSpacing);
                }
                else//kullan�c�n�n leaderboarda kay�tl� puan� varsa
                {
                    int Limit = 10;
                    int page = 0;
                    int Offset = Limit * page;
                    int rangeLimit = 5;

                    //oyuncu ilk 10 listesinde de�ilse. +rangelimit dememizin sebebi kullan�c�n�n �st�nde ve alt�ndaki 5 oyuncuyu daha listeleyecek olmam�z.
                    if (scoreResponse.Rank >= Limit + rangeLimit)
                    {
                        LeaderboardScoresPage topScores = await LeaderboardsService.Instance.GetScoresAsync(GlobalVariables.LeaderboardId_Richest, new GetScoresOptions { Offset = Offset, Limit = Limit });

                        // top 10 bilgileri
                        for (int i = 0; i < topScores.Results.Count; i++)
                        {
                            FillLeaderboardRow(i, topScores.Results[i], new Color(9f / 255f, 183f / 255f, 255f / 255f, 0f / 255f));
                        }
                        //bir ayra� at�yoruz.
                        GameObject newBrace = Instantiate(TemplateBrace, RowParent.transform);
                        newBrace.SetActive(true);
                        LastRowIndex++;
                        newBrace.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, LastRowIndex * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing) + LineSpacing + rowGroupSpacing);
                        rowGroupSpacing = -(newBrace.GetComponent<RectTransform>().sizeDelta.y - (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing));

                        // kullan�c� ve +- 5 mesafesi skorlar�
                        LeaderboardScores rangedScores = await LeaderboardsService.Instance.GetPlayerRangeAsync(GlobalVariables.LeaderboardId_Richest, new GetPlayerRangeOptions { RangeLimit = rangeLimit });

                        int tempIndex = LastRowIndex + 1;
                        for (int i = 0; i < rangedScores.Results.Count; i++)
                        {
                            if (scoreResponse.Rank == rangedScores.Results[i].Rank)
                            {
                                //kendi ad�n� farkl� renkte g�stermek i�in kullan�l�yordu. Ancak async �al��t��� i�in g�sterirken s�ralama de�i�ebiliyor ve yanl�� ki�i highlight edilebiliyor.
                                FillLeaderboardRow(tempIndex + i, rangedScores.Results[i], new Color(83f / 255f, 159f / 255f, 91f / 255f));
                                //FillLeaderboardRow(tempIndex + i, rangedScores.Results[i], Color.white);
                            }
                            else
                            {
                                FillLeaderboardRow(tempIndex + i, rangedScores.Results[i], new Color(0, 0, 0, 0));
                            }
                        }

                        //E�er kullan�c�dan sonra listelenen 5 ki�iden ba�ka ki�ilerde varsa bir ayra� daha at�yoruz
                        if (topScores.Total > rangedScores.Results[rangedScores.Results.Count - 1].Rank + 1)
                        {
                            //bir ayra� daha at�yoruz
                            newBrace = Instantiate(TemplateBrace, RowParent.transform);
                            newBrace.SetActive(true);
                            LastRowIndex++;
                            newBrace.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, LastRowIndex * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing) + LineSpacing + rowGroupSpacing);
                            rowGroupSpacing += -(newBrace.GetComponent<RectTransform>().sizeDelta.y - (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing));
                        }

                        //toplam kay�t say�s� Text'i
                        LastRowIndex++;
                        TotalRecordText.SetActive(true);
                        TotalRecordText.GetComponent<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Total") + ": " + topScores.Total.ToString();
                        TotalRecordText.GetComponent<RectTransform>().anchoredPosition = new Vector2(TotalRecordText.GetComponent<RectTransform>().anchoredPosition.x, nextPosY + -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing) + rowGroupSpacing);
                    }
                    else//oyuncu puan olarak ilk 10'da ise
                    {
                        Limit = 30;
                        page = 0;
                        Offset = Limit * page;

                        LeaderboardScoresPage topScores = await LeaderboardsService.Instance.GetScoresAsync(GlobalVariables.LeaderboardId_Richest, new GetScoresOptions { Offset = Offset, Limit = Limit });

                        for (int i = 0; i < topScores.Results.Count; i++)
                        {
                            if (topScores.Results[i].Rank == scoreResponse.Rank)
                            {
                                //kendi ad�n� farkl� renkte g�stermek i�in kullan�l�yordu. Ancak async �al��t��� i�in g�sterirken s�ralama de�i�ebiliyor ve yanl�� ki�i highlight edilebiliyor.
                                FillLeaderboardRow(i, topScores.Results[i], new Color(83f / 255f, 159f / 255f, 91f / 255f));
                                //FillLeaderboardRow(i, topScores.Results[i], Color.white);
                            }
                            else
                            {
                                FillLeaderboardRow(i, topScores.Results[i], new Color(0, 0, 0, 0));
                            }
                        }

                        float newBraceSpacing = 0;
                        //E�er toplam kay�t say�s� listelenenden fazla ise bir ayra� at�yoruz.
                        if (topScores.Total > Limit)
                        {
                            GameObject newBrace = Instantiate(TemplateBrace, RowParent.transform);
                            newBrace.SetActive(true);
                            newBrace.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (topScores.Results.Count * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing)) + LineSpacing);
                            newBraceSpacing = newBrace.GetComponent<RectTransform>().sizeDelta.y - LineSpacing;
                        }

                        //toplam kay�t say�s� Text'i
                        TotalRecordText.SetActive(true);
                        TotalRecordText.GetComponent<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Total") + ": " + topScores.Total.ToString();
                        TotalRecordText.GetComponent<RectTransform>().anchoredPosition = new Vector2(TotalRecordText.GetComponent<RectTransform>().anchoredPosition.x, (topScores.Results.Count * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing)) - newBraceSpacing);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("Leaderboard listesi doldurulurken bir hata meydana geldi. " + e);
            LoadAnimation.GetComponent<LoadSaveAnimationController>().StopAnimation();
        }
        finally
        {
            SetContentHeight();
            LoadAnimation.GetComponent<LoadSaveAnimationController>().StopAnimation();
            Canvas.ForceUpdateCanvases();
        }
    }

    void FillLeaderboardRow(int rowIndex, LeaderboardEntry content, Color backgroundColor)
    {
        if (rowIndex > LastRowIndex)
            LastRowIndex = rowIndex;

        GameObject newRow = Instantiate(TemplateRow, RowParent.transform);
        newRow.SetActive(true);
        nextPosY = LastRowIndex * -1 * (TemplateRow.GetComponent<RectTransform>().sizeDelta.y + LineSpacing) + rowGroupSpacing;
        newRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, nextPosY);
        newRow.transform.GetChild(0).GetComponent<Image>().color = backgroundColor;
        newRow.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = (content.Rank + 1).ToString();
        newRow.transform.GetChild(1).transform.GetChild(1).GetComponent<TMP_Text>().text = content.PlayerName.Substring(0, content.PlayerName.IndexOf('#'));
        newRow.transform.GetChild(1).transform.GetChild(2).GetComponent<TMP_Text>().text = content.Score.ToString();
    }

    void SetContentHeight()
    {
        ScrollviewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(ScrollviewContent.GetComponent<RectTransform>().sizeDelta.x, Mathf.Abs(TotalRecordText.GetComponent<RectTransform>().anchoredPosition.y) + TotalRecordText.GetComponent<RectTransform>().sizeDelta.y + 50);
    }
}
