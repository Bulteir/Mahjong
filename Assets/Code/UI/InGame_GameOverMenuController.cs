using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class InGame_GameOverMenuController : MonoBehaviour
{
    public TMP_Text Title;
    public TMP_Text Content;
    public GameObject NextLevelButton;
    public GameObject Timer;
    public GameObject x2Ad;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetContent(string title, int reward, bool levelIsWon)
    {
        Title.text = title;
        string contentString = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Time") + ": " + Timer.GetComponent<Timer>().text.text;
        Content.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Time") + ": " + Timer.GetComponent<Timer>().text.text;
        
        if (levelIsWon)//bölüm kazanýlmýþ
        {
            NextLevelButton.SetActive(true);
            contentString += "\n" + LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Reward") + ": " + reward.ToString();
        }
        else//bölüm kaybedilmiþ
        {
            NextLevelButton.SetActive(false);
            x2Ad.SetActive(false);
        }

        Content.text = contentString;
    }

}
