using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPurhasePopup_Controller : MonoBehaviour
{
    public TMP_Text Content;
    public Button Confirm;

    public void ShowPopup (SaveDataFormat saveFile,int levelNumber)
    {
        gameObject.SetActive(true);
        Content.text = "Bölümü satýn al. " + (GlobalVariables.LevelRewards[levelNumber - 1] * 3).ToString() + " coin";
        Confirm.GetComponent<LevelPurhasePopup_Confirm_Btn>().SetConfirmButton(saveFile, levelNumber);
    }

}
