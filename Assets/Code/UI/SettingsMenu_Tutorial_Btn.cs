using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu_Tutorial_Btn : MonoBehaviour
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
            saveFile.tutorialPassed = true;
            saveFile.saveTime = DateTime.Now.ToString();
            generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
        }
        GlobalVariables.tutorialCalledSettingMenu = true;
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }
}
