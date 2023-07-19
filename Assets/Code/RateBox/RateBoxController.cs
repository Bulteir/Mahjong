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
    //kullan�c�n�n oy verip vermedi�ini kontrol edebilme y�ntemi olmad��� i�in (herhangi bir callback yokmu�) oy verme ekran� a��l�nca oy vermi� sayaca��z.
    bool ratedAlredy = false;

    //kullan�c�n�n no thanks butonuna bas�p basmad���n� tutar
    bool doesPlayerWantToRate = true;

    //Ka� kez giri� yap�ld�ktan sonra oylama kutusu a��lacak 
    public int countToRate = 0;

    //onay red daha sonra butonlar�n�n parent�
    public Transform buttons;

    void Awake()
    {
        SetMinFontSizeAllButtons();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    //oyun a��ld���nda �al��an bir scriptin start fonskiyonundan �a�r�lmal�
    public void RateBoxControlAtStartGame()
    {
        //Test i�in
        //PlayerPrefs.DeleteKey("PlayCountForRate");
        //PlayerPrefs.DeleteKey("DoesPlayerWantToRate");
        //PlayerPrefs.DeleteKey("DoesPlayerRatedGame");

        #region kullan�c� no thanks butonuna basm�� m� kontrol�.
        string doesPlayerWantToRateString = PlayerPrefs.GetString("DoesPlayerWantToRate");
        if (doesPlayerWantToRateString == "")
        {
            doesPlayerWantToRateString = "true";
        }
        bool.TryParse(doesPlayerWantToRateString, out doesPlayerWantToRate);
        #endregion

        #region kullan�c� daha �nce oy verme ekran�n� a�t� m�? yani oy verdi sayacak m�y�z?
        string ratedAlredyString = PlayerPrefs.GetString("DoesPlayerRatedGame");
        if (ratedAlredyString == "")
        {
            ratedAlredyString = "false";
        }
        bool.TryParse(ratedAlredyString, out ratedAlredy);
        #endregion

        #region oyunu belirli miktar tekrar a�t�ktan sonra kullan�c�n�n kar��s�na oy verme ekran� ��kart

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
        //buras� oyun markete ��k�nca de�i�ecek
        Application.OpenURL("market://details?id=com.Alpay.BlockPuzzle");

        ratedAlredy = true;
        PlayerPrefs.SetString("DoesPlayerRatedGame", ratedAlredy.ToString());
        PlayerPrefs.Save();
        gameObject.SetActive(false);
    }

    //ratebox butonlar�n�n font b�y�kl���n� en d���k font b�y�kl���n� bularak t�m butonlara uygular
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
