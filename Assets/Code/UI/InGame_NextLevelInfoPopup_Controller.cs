using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGame_NextLevelInfoPopup_Controller : MonoBehaviour
{
    public TMP_Text Info;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowPopup(int nextLevelNumber)
    {
        gameObject.SetActive(true);

        Dictionary<string, string> arguments = new Dictionary<string, string> { { "levelName", nextLevelNumber.ToString() }, { "coin", (GlobalVariables.LevelRewards[nextLevelNumber - 1] * 3).ToString() } };
        Info.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Next Level Not Enough Gold", new object[] { arguments });
    }
}
