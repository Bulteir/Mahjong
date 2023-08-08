using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

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
        int currentlevelIndex = Int32.Parse(SceneManager.GetActiveScene().name.Replace("level", ""));

        Title.text = title;
        string contentString = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Time") + ": " + Timer.GetComponent<Timer>().text.text;
        Content.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Time") + ": " + Timer.GetComponent<Timer>().text.text;
        
        if (levelIsWon)//b�l�m kazan�lm��
        {
            if (currentlevelIndex < GlobalVariables.LevelRewards.Count)
            {
                NextLevelButton.SetActive(true);
            }
            else
            {
                NextLevelButton.SetActive(false);
            }
            contentString += "\n" + LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Reward") + ": " + reward.ToString();
        }
        else//b�l�m kaybedilmi�
        {
            NextLevelButton.SetActive(false);
            x2Ad.SetActive(false);
        }

        Content.text = contentString;
    }

}
