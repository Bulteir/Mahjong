using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using TMPro;

public class LocalSaveLoadController : MonoBehaviour
{
    IDataService DataService = new JsonDataService();
    bool EncryptionEnabled = true;
    long saveTime;
    long loadTime;

    //Bu deneme fonksiyonu i�inde save,load yap�yor ve s�relerini �l��yor.
    public void SerializeJson()
    {
        //Bu �ekilde veri setliyoruz.
        SaveDataFormat saveData = new SaveDataFormat();
        saveData.unlockedLevelNumber = 2;
        saveData.test = "oldu yey";

        long startTime = DateTime.Now.Ticks;

        if (DataService.SaveData("/save.json", saveData, EncryptionEnabled))
        {
            saveTime = DateTime.Now.Ticks - startTime;
            Debug.Log($"Data saved. Save Time: {(saveTime / 1000f):N4}ms");

            startTime = DateTime.Now.Ticks;
            try
            {
                SaveDataFormat data = DataService.LoadData<SaveDataFormat>("/save.json", EncryptionEnabled);
                loadTime = DateTime.Now.Ticks - startTime;
                Debug.Log("Loaded Game: " + JsonConvert.SerializeObject(data, Formatting.Indented));
                Debug.Log($"Load Time: {(loadTime / 1000f):N4}ms");
            }
            catch(Exception e)
            {
                Debug.LogError($"Could not read file!");
                throw e;
            }
        }
        else
        {
            Debug.LogError("Could not save file!");
        }
    }

    //oyunu kaydetmek istedi�mizde b�yle bir fonksiyon yazaca��z
    public void SaveGame()
    {
        //Bu �ekilde veri setliyoruz.
        SaveDataFormat saveData = new SaveDataFormat();
        saveData.unlockedLevelNumber = 2;
        saveData.test = "oldu yey";

        if (DataService.SaveData("/save.json", saveData, EncryptionEnabled))
        {
            Debug.Log("Game saved.");
        }
        else
        {
            Debug.LogError("Could not save game!");
        }
    }

    //Kaydedilmi� oyun dosyas�n� almak istedi�imizde b�yle bir fonksiyon yazaca��z
    public void LoadGame()
    {
        try
        {
            //istedi�imiz class hali
            SaveDataFormat data = DataService.LoadData<SaveDataFormat>("/save.json", EncryptionEnabled);
            //string hali
            string gameData = JsonConvert.SerializeObject(data, Formatting.Indented);
            Debug.Log("Game Loaded");
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not Load Game!");
            throw e;
        }
    }
}
