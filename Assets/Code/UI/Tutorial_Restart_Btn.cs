using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_Restart_Btn : MonoBehaviour
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
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }
}
