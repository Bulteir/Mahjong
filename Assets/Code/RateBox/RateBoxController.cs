using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class RateBoxController : MonoBehaviour
{
    //kullanýcýnýn oy verip vermediðini kontrol edebilme yöntemi olmadýðý için (herhangi bir callback yokmuþ) oy verme ekraný açýlýnca oy vermiþ sayacaðýz.
    bool ratedAlredy = false;

    //kullanýcýnýn no thanks butonuna basýp basmadýðýný tutar
    bool doesPlayerWantToRate = true;

    //Kaç kez giriþ yapýldýktan sonra oylama kutusu açýlacak 
    public int countToRate = 0;

    //onay red daha sonra butonlarýnýn parentý
    public Transform buttons;

    void Awake()
    {
        SetMinFontSizeAllButtons();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    //oyun açýldýðýnda çalýþan bir scriptin start fonskiyonundan çaðrýlmalý
    public void RateBoxControlAtStartGame()
    {
        //Test için
        //PlayerPrefs.DeleteKey("PlayCountForRate");
        //PlayerPrefs.DeleteKey("DoesPlayerWantToRate");
        //PlayerPrefs.DeleteKey("DoesPlayerRatedGame");

        #region kullanýcý no thanks butonuna basmýþ mý kontrolü.
        string doesPlayerWantToRateString = PlayerPrefs.GetString("DoesPlayerWantToRate");
        if (doesPlayerWantToRateString == "")
        {
            doesPlayerWantToRateString = "true";
        }
        bool.TryParse(doesPlayerWantToRateString, out doesPlayerWantToRate);
        #endregion

        #region kullanýcý daha önce oy verme ekranýný açtý mý? yani oy verdi sayacak mýyýz?
        string ratedAlredyString = PlayerPrefs.GetString("DoesPlayerRatedGame");
        if (ratedAlredyString == "")
        {
            ratedAlredyString = "false";
        }
        bool.TryParse(ratedAlredyString, out ratedAlredy);
        #endregion

        #region oyunu belirli miktar tekrar açtýktan sonra kullanýcýnýn karþýsýna oy verme ekraný çýkart

        string playCountString = PlayerPrefs.GetString("PlayCountForRate");
        if (playCountString == "")
        {
            playCountString = "0";
        }

        int playCount = -1;
        int.TryParse(playCountString, out playCount);

        if (GlobalVariables.internetAvaible)
        {
            if (playCount != -1)
            {
                playCount++;

                if (playCount % countToRate == 0 && ratedAlredy == false && doesPlayerWantToRate == true)
                {
#if UNITY_ANDROID
                    gameObject.SetActive(true);
#elif UNITY_IOS
                Device.RequestStoreReview();
                ratedAlredy = true;
                PlayerPrefs.SetString("DoesPlayerRatedGame", ratedAlredy.ToString());
                PlayerPrefs.Save();     
#endif
                }
            }
        }
        #endregion

        PlayerPrefs.SetString("PlayCountForRate", playCount.ToString());
        PlayerPrefs.SetString("DoesPlayerWantToRate", doesPlayerWantToRateString.ToString());
        PlayerPrefs.SetString("DoesPlayerRatedGame", ratedAlredy.ToString());
        PlayerPrefs.Save();
    }

    public void ClickNoThanksBtn()
    {
        PlayerPrefs.SetString("DoesPlayerWantToRate", "false");
        gameObject.SetActive(false);
    }
    public void ClickLaterBtn()
    {
        gameObject.SetActive(false);
    }
    public void ClickRateNowBtn()
    {
        //burasý oyun markete çýkýnca deðiþecek
        Application.OpenURL("market://details?id=com.Alpay.BlockPuzzle");

        ratedAlredy = true;
        PlayerPrefs.SetString("DoesPlayerRatedGame", ratedAlredy.ToString());
        PlayerPrefs.Save();
        gameObject.SetActive(false);
    }

    //ratebox butonlarýnýn font büyüklüðünü en düþük font büyüklüðünü bularak tüm butonlara uygular
    void SetMinFontSizeAllButtons()
    {
        float minFontSize = Int32.MaxValue;

        foreach (Transform button in buttons)
        {
            button.GetComponentInChildren<TMP_Text>().enableAutoSizing = true;
            buttons.GetComponentInChildren<TMP_Text>().ForceMeshUpdate();


            if (button.GetComponentInChildren<TMP_Text>().fontSize < minFontSize)
            {
                minFontSize = button.GetComponentInChildren<TMP_Text>().fontSize;
            }
        }

        foreach (Transform button in buttons)
        {
            button.GetComponentInChildren<TMP_Text>().enableAutoSizing = false;
            minFontSize = button.GetComponentInChildren<TMP_Text>().fontSize = minFontSize;
        }
    }
}
