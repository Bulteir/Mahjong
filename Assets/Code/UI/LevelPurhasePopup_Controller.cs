using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LevelPurhasePopup_Controller : MonoBehaviour
{
    public TMP_Text Content;
    public Button Confirm;

    public void ShowPopup (SaveDataFormat saveFile,int levelNumber)
    {
        gameObject.SetActive(true);

        Dictionary<string, string> arguments = new Dictionary<string, string> { { "levelName", levelNumber.ToString() }, { "coin", (GlobalVariables.LevelRewards[levelNumber - 1] * 3).ToString() } };
        Content.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Level Purchase Content", new object[] { arguments });
        Confirm.GetComponent<LevelPurhasePopup_Confirm_Btn>().SetConfirmButton(saveFile, levelNumber);
    }

}
