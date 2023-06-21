using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Settings_Btn : MonoBehaviour
{
    public GameObject generalControllers;
    public void OnClick()
    {
        //Settings menüsü açýlýrken save dosyasý yoksa default save dosyasý oluþturulur.
        generalControllers.GetComponent<GameSaveLoadController>().CreateDefaultSaveFile();
        GlobalVariables.gameState = GlobalVariables.gameState_SettingsMenu;
    }
}
