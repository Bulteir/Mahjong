using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_StartGame_Btn : MonoBehaviour
{
    public GameObject generalControllers;
    public void OnClick ()
    {
        //oyun açýldýðýnda local save dosyasý yoksa save dosyasý oluþturulur.
        
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime == null)//Kayýtlý save dosyasý yoksa
        {
            saveFile = new SaveDataFormat();

            //default deðer atamalarý burada yapýlýr
            saveFile.totalCoin = GlobalVariables.FirstTotalCoin;
            saveFile.shuffleJokerQuantity = GlobalVariables.FirstShuffleJokerQuantity;
            saveFile.undoJokerQuantity = GlobalVariables.FirstShuffleJokerQuantity;
            saveFile.saveFileIsSyncEver = false;
            saveFile.totalEnergy = GlobalVariables.maxEnergy;
            saveFile.noAdsJokerActive = false;
            saveFile.lastEnergyGainTime = System.DateTime.Now.ToString();
            saveFile.unlimitedEnergyActive = false;
            saveFile.unlimitedEnergyEndTime = System.DateTime.Now.ToString();

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
