using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Settings_Btn : MonoBehaviour
{
    public GameObject generalControllers;
    public void OnClick()
    {
        //Settings men�s� a��l�rken save dosyas� yoksa default save dosyas� olu�turulur.
        generalControllers.GetComponent<GameSaveLoadController>().CreateDefaultSaveFile();
        GlobalVariables.gameState = GlobalVariables.gameState_SettingsMenu;
    }
}
