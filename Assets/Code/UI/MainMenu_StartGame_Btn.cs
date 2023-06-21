using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_StartGame_Btn : MonoBehaviour
{
    public GameObject generalControllers;
    public void OnClick ()
    {
        //oyun açýldýðýnda local save dosyasý yoksa save dosyasý oluþturulur.
        generalControllers.GetComponent<GameSaveLoadController>().CreateDefaultSaveFile();
        
        GlobalVariables.gameState = GlobalVariables.gameState_LevelSelectmenu;
    }
}
