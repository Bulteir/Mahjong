using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_StartGame_Btn : MonoBehaviour
{
    public GameObject generalControllers;
    public void OnClick ()
    {
        //oyun a��ld���nda local save dosyas� yoksa save dosyas� olu�turulur.
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime == null)//Kay�tl� save dosyas� varsa
        {
            saveFile = new SaveDataFormat();
            saveFile.totalCoin = GlobalVariables.FirstTotalCoin;
            saveFile.saveFileIsSyncEver = false;
            saveFile.saveTime = System.DateTime.Now.ToString();

            saveFile.levelProperties = new List<LevelProperties> { new LevelProperties
            {
                LevelName = "level1",
                levelPassed = false,
                levelPurchased = true,
            } };

            generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
        }
        GlobalVariables.gameState = GlobalVariables.gameState_LevelSelectmenu;
    }
}
