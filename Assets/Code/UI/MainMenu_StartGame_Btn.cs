using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_StartGame_Btn : MonoBehaviour
{
    public GameObject generalControllers;
    public void OnClick ()
    {
        //oyun a��ld���nda local save dosyas� yoksa save dosyas� olu�turulur.
        generalControllers.GetComponent<GameSaveLoadController>().CreateDefaultSaveFile();
        
        GlobalVariables.gameState = GlobalVariables.gameState_LevelSelectmenu;
    }
}
