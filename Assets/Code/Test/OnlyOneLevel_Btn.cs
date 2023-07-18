using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnlyOneLevel_Btn : MonoBehaviour
{
    public GameObject generalControllers;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnClick()
    {
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            for (int i = 0; i < saveFile.levelProperties.Count; i++)
            {
                if (saveFile.levelProperties[i].LevelName != "level1")
                {

                    LevelProperties levelProperties = new LevelProperties();
                    levelProperties = saveFile.levelProperties[i];

                    saveFile.levelProperties.Remove(levelProperties);

                    levelProperties.bestTime = "";
                    levelProperties.levelPassed = false;
                    levelProperties.earnedStarQuantity = 0;
                    levelProperties.levelPurchased = false;
                    saveFile.levelProperties.Add(levelProperties);
                }   
            }
            saveFile.saveTime = DateTime.Now.ToString();
            generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
        }
    }
}
