using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UIElements;

public class GameSaveLoadController : MonoBehaviour
{
    SaveDataFormat saveData;
    // Start is called before the first frame update
    void Start()
    {
        saveData = new SaveDataFormat();
        saveData.unlockedLevelNumber = 3;
        saveData.test = "Save load sistemi tamamlandý";
        saveData.saveTime = DateTime.Now.ToShortDateString() +" "+ DateTime.Now.ToShortTimeString();


        ////local load örneði
        //SaveDataFormat loadData = LoadGameLocal();
        //string gameDataJson = JsonConvert.SerializeObject(loadData, Formatting.Indented);
    }

    public void SaveGameLocal(SaveDataFormat data)
    {
        GetComponent<LocalSaveLoadController>().SaveGame(data);
    }

    public void SaveGameCloud(SaveDataFormat data)
    {
        if (GlobalVariables.cloudSaveSystemIsReady == true)
        {
            GetComponent<CloudSaveController>().SaveData(JsonConvert.SerializeObject(data));
        }
        else
        {
            Debug.Log("Unity Cloud Save servisi hazýr deðil. Veriler buluta yüklenemedi.");
        }
    }

    public SaveDataFormat LoadGameLocal()
    {
        SaveDataFormat data = new SaveDataFormat();
        data = GetComponent<LocalSaveLoadController>().LoadGame();
        return data;
    }

    public async void LoadGameCloud()
    {
        string data = await GetComponent<CloudSaveController>().LoadData();
        Debug.Log("Cloud Load Data:" + data);
    }

    public void TempLocalSaveClick()
    {
        SaveGameLocal(saveData);
    }

    public void TempCloudSaveClick()
    {
        SaveGameCloud(saveData);
    }

    public void TempLocalLoadClick()
    {
        SaveDataFormat data = LoadGameLocal();
        Debug.Log("Local Load data, unlockedLevelNumber:" + data.unlockedLevelNumber + " test:" + data.test + " SaveTime:" + data.saveTime);
    }
}
