using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UIElements;
using System.IO;

public class GameSaveLoadController : MonoBehaviour
{
    SaveDataFormat saveData;
    public GameObject LoadAnimation;
    // Start is called before the first frame update
    void Start()
    {
        saveData = new SaveDataFormat();
        saveData.unlockedLevelNumber = 3;
        saveData.test = "Save load sistemi tamamland�";
        saveData.saveTime = DateTime.Now.ToString();

        ////local load �rne�i
        //SaveDataFormat loadData = LoadGameLocal();
        //string gameDataJson = JsonConvert.SerializeObject(loadData, Formatting.Indented);

    }

    IEnumerator SaveGameLocal(SaveDataFormat data)
    {
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StartAnimation("Kaydediliyor");
        yield return null;
        GetComponent<LocalSaveLoadController>().SaveGame(data);
        yield return null;
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StopAnimation();
        yield return null;
    }

    IEnumerator SaveGameCloud(SaveDataFormat data)
    {
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StartAnimation("Kaydediliyor");
        yield return null;
        if (GlobalVariables.cloudSaveSystemIsReady == true)
        {
            GetComponent<CloudSaveController>().SaveData(JsonConvert.SerializeObject(data));
            yield return null;
        }
        else
        {
            Debug.Log("Unity Cloud Save servisi haz�r de�il. Veriler buluta y�klenemedi.");
        }
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StopAnimation();
        yield return null;
    }

    SaveDataFormat LoadGameLocal()
    {
        return GetComponent<LocalSaveLoadController>().LoadGame();
    }

    IEnumerator LoadGameLocalCorroutine()
    {
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StartAnimation("Y�kleniyor");
        yield return null;
        SaveDataFormat data = LoadGameLocal();
        Debug.Log("Local Load data:"+(data.saveTime==null?false:data.saveTime)+" unlockedLevelNumber:" + data.unlockedLevelNumber + " test:" + data.test + " SaveTime:" + data.saveTime);
        yield return null;
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StopAnimation();
        yield return null;
    }

    async void LoadGameCloud()
    {
        string data = await GetComponent<CloudSaveController>().LoadData();
        Debug.Log("Cloud Load Data:" + data);
    }

    IEnumerator LoadGameCloudCorroutine()
    {
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StartAnimation("Y�kleniyor");
        yield return null;
        LoadGameCloud();
        yield return null;
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StopAnimation();
        yield return null;
    }

    public async void GameSaveDataSynchronization()
    {
        if (GlobalVariables.cloudSaveSystemIsReady == true)
        {
            LoadAnimation.GetComponent<LoadSaveAnimationController>().StartAnimation("Senkronize ediliyor.");
            string data = await GetComponent<CloudSaveController>().LoadData();

            //local save dosyas�n� okuyoruz.
            SaveDataFormat formattedLocalData = new SaveDataFormat();
            formattedLocalData = LoadGameLocal();

            //Cloudda herhangi bir kay�t varsa
            if (data != "")
            {
                //json cloud datas�n� formatl�yoruz
                SaveDataFormat formattedCloudData = JsonConvert.DeserializeObject<SaveDataFormat>(data);

                //local kay�t dosyas� da varsa
                if (formattedLocalData.saveTime != null)
                {
                    DateTime cloudSaveTime;
                    DateTime.TryParse(formattedCloudData.saveTime,out cloudSaveTime);

                    DateTime localSaveTime;
                    DateTime.TryParse(formattedLocalData.saveTime, out localSaveTime);

                    //Local kay�t daha yeni
                    if (cloudSaveTime.CompareTo(localSaveTime) < 0)
                    {
                        //local save dosyas�n� buluta at�yoruz
                        GetComponent<CloudSaveController>().SaveData(JsonConvert.SerializeObject(formattedLocalData));
                    }
                    else //Cloud kayd� daha yeni veya localle ayn�
                    {
                        //Cloud kayd�n� locale kaydediyoruz.
                        GetComponent<LocalSaveLoadController>().SaveGame(formattedCloudData);
                    }
                }
                else//local kay�t dosyas� yoksa
                {
                    GetComponent<LocalSaveLoadController>().SaveGame(formattedCloudData);
                }
            }
            else//Cloudda herhangi bir kay�t yoksa
            {
                //local save dosyas� varsa
                if (formattedLocalData.saveTime != null)
                {
                    //local save dosyas�n� buluta at�yoruz
                    GetComponent<CloudSaveController>().SaveData(JsonConvert.SerializeObject(formattedLocalData));
                }
            }
            LoadAnimation.GetComponent<LoadSaveAnimationController>().StopAnimation();
            Debug.Log("Kay�t dosyas� senkronizasyonu tamamland�.");
        }
        else
        {
            Debug.Log("Kay�t dosyas� senkronizasyonu i�in unity cloud sistemi haz�r de�il");
        }
    }

    public void LocalSaveButtonClick()
    {
        StartCoroutine(SaveGameLocal(saveData));
    }

    public void CloudSaveButtonClick()
    {
        StartCoroutine(SaveGameCloud(saveData));
    }

    public void LocalLoadButtonClick()
    {
        StartCoroutine(LoadGameLocalCorroutine());
    }

    public void CloudLoadButtonClick()
    {
        StartCoroutine(LoadGameCloudCorroutine());
    }
}
