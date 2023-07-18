using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGame_StarController : MonoBehaviour
{
    [Tooltip("Bir yıldız kazanmak için kaç saniye gereklidir.")]
    public int requiredSecondForGainStar;
    [Tooltip("Bölüm içerisinde şu ana kadar kaç yıldız kazanıldı.")]
    public int gainedStarQuantity = 0;
    [Tooltip("Bölüm toplam kaç blok ile başladığı setlenir.")]
    public int startBlockCount;

    public Image Star1;
    public Image Star2;
    public Image Star3;

    public Sprite starOn;
    public Sprite starOff;

    public TMP_Text timer;
    public TMP_Text blockCounter;

    bool phase1Passed = false;
    bool phase2Passed = false;
    bool phase3Passed = false;
    string phasePassTime = "00:00:00";

    public AudioSource StarGainSound;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetStars()
    {
        string playTimeFormat = "mm:ss:ff";
        if (timer.text.Length > 8)//saatte gösterir
        {
            playTimeFormat = "HH:mm:ss:ff";
        }

        DateTime playTime;
        DateTime.TryParseExact(timer.text, playTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out playTime);

        DateTime currentTime;
        DateTime.TryParseExact(phasePassTime, "mm:ss:ff", CultureInfo.InvariantCulture, DateTimeStyles.None, out currentTime);

        int leftBlockCount = Int32.Parse(blockCounter.text);
        int matchedBlockCount = startBlockCount - leftBlockCount;

        float status = (float)matchedBlockCount / (float)startBlockCount;

        if (status > 0.25f && status <= 0.5f && phase1Passed == false)
        {
            if ((playTime - currentTime).TotalSeconds < requiredSecondForGainStar)
            {
                gainedStarQuantity++;
            }
            phase1Passed = true;
            phasePassTime = timer.text;
            StarGainSound.Play();
        }
        else if (status > 0.5f && status <= 0.75f && phase2Passed == false)
        {
            if ((playTime - currentTime).TotalSeconds < requiredSecondForGainStar)
            {
                gainedStarQuantity++;
            }
            phase2Passed = true;
            phasePassTime = timer.text;
            StarGainSound.Play();

        }
        else if (status > 0.75f && status <= 1 && phase3Passed == false)
        {
            if ((playTime - currentTime).TotalSeconds < requiredSecondForGainStar)
            {
                gainedStarQuantity++;
            }
            phase3Passed = true;
            phasePassTime = timer.text;
            StarGainSound.Play();

        }

        if (gainedStarQuantity == 1)
        {
            Star1.sprite = starOn;
        }
        else if (gainedStarQuantity == 2)
        {
            Star2.sprite = starOn;
        }
        else if (gainedStarQuantity == 3)
        {
            Star3.sprite = starOn;
        }
    }
}
