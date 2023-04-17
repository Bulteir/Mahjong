using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using TMPro;
using Unity.Services.CloudSave;

public class LocalSaveLoadController : MonoBehaviour
{
    IDataService DataService = new JsonDataService();
    bool EncryptionEnabled = true;
    long saveTime;
    long loadTime;

    //Bu deneme fonksiyonu içinde save,load yapýyor ve sürelerini ölçüyor.
    //public void SerializeJson()
    //{
    //    //Bu þekilde veri setliyoruz.
    //    SaveDataFormat saveData = new SaveDataFormat();
    //    saveData.unlockedLevelNumber = 2;
    //    saveData.test = "oldu yey";

    //    long startTime = DateTime.Now.Ticks;

    //    if (DataService.SaveData("/save.json", saveData, EncryptionEnabled))
    //    {
    //        saveTime = DateTime.Now.Ticks - startTime;
    //        Debug.Log($"Data saved. Save Time: {(saveTime / 1000f):N4}ms");

    //        startTime = DateTime.Now.Ticks;
    //        try
    //        {
    //            SaveDataFormat data = DataService.LoadData<SaveDataFormat>("/save.json", EncryptionEnabled);
    //            loadTime = DateTime.Now.Ticks - startTime;
    //            Debug.Log("Loaded Game: " + JsonConvert.SerializeObject(data, Formatting.Indented));
    //            Debug.Log($"Load Time: {(loadTime / 1000f):N4}ms");
    //        }
    //        catch(Exception e)
    //        {
    //            Debug.LogError($"Could not read file!");
    //            throw e;
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Could not save file!");
    //    }
    //}

    //oyunu kaydetmek istediðmizde böyle bir fonksiyon yazacaðýz
    public void SaveGame(SaveDataFormat data)
    {
        if (DataService.SaveData("/save.json", data, EncryptionEnabled))
        {
            Debug.Log("Game saved.");
        }
        else
        {
            Debug.LogError("Could not save game!");
        }
    }

    //Kaydedilmiþ oyun dosyasýný almak istediðimizde böyle bir fonksiyon yazacaðýz
    public SaveDataFormat LoadGame()
    {
        SaveDataFormat data = new SaveDataFormat();
        try
        {
            //istediðimiz class hali
            data = DataService.LoadData<SaveDataFormat>("/save.json", EncryptionEnabled);
            //string hali
            //string gameData = JsonConvert.SerializeObject(data, Formatting.Indented);

            Debug.Log("Game Loaded");
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not Load Game! " + e.Message);
            //throw e;
            return data;
        }
    }
}
