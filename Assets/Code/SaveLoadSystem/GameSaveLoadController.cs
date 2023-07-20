using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UIElements;
using System.IO;
using UnityEngine.Localization.Settings;
using Unity.VisualScripting;

public class GameSaveLoadController : MonoBehaviour
{
    SaveDataFormat saveData;
    public GameObject LoadAnimation;
    public GameObject SaveFileSyncPopup;
    // Start is called before the first frame update
    void Start()
    {

    }

    //�rnek kod. Test butonlar�nda kullan�l�yor
    IEnumerator SaveGameLocal(SaveDataFormat data)
    {
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StartAnimation(LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Saving"));
        yield return null;
        GetComponent<LocalSaveLoadController>().SaveGame(data);
        yield return null;
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StopAnimation();
        yield return null;
    }

    //�rnek kod. Test butonlar�nda kullan�l�yor
    IEnumerator SaveGameCloud(SaveDataFormat data)
    {
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StartAnimation(LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Saving"));
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

    //�rnek kod. Test butonlar�nda kullan�l�yor
    IEnumerator LoadGameLocalCorroutine()
    {
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StartAnimation(LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Loading"));
        yield return null;
        SaveDataFormat data = LoadGameLocal();
        Debug.Log("Local Load data:" + (data.saveTime == null ? false : data.saveTime));
        yield return null;
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StopAnimation();
        yield return null;
    }

    async void LoadGameCloud()
    {
        string data = await GetComponent<CloudSaveController>().LoadData();
        Debug.Log("Cloud Load Data:" + data);
    }

    //�rnek kod. Test butonlar�nda kullan�l�yor
    IEnumerator LoadGameCloudCorroutine()
    {
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StartAnimation(LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Loading"));
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
            LoadAnimation.GetComponent<LoadSaveAnimationController>().StartAnimation(LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Syncing"));
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
                    if (formattedLocalData.saveFileIsSyncEver == false)//oyun daha �nce buluta kaydedilmi� ve silinip tekrar y�klenmi�se buraya gelir. Ya da cihaz de�i�tirme gibi durumda
                    {
                        SaveFileSyncPopup.GetComponent<SaveFileSyncPopupController>().localSaveFile = formattedLocalData;
                        SaveFileSyncPopup.GetComponent<SaveFileSyncPopupController>().cloudSaveFile = formattedCloudData;
                        SaveFileSyncPopup.GetComponent<SaveFileSyncPopupController>().ShowPopup();
                    }
                    else
                    {
                        //Bu k�s�m save dosyalar�n�n sadece kay�t tarihilerini kar��la�t�rarak en yeni olan� tutarak �al���r.
                        DateTime cloudSaveTime;
                        DateTime.TryParse(formattedCloudData.saveTime, out cloudSaveTime);

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

                            # region cloud verisi kaydedildi�i i�in coin bar ve enerji bar save dosayas�na g�re d�zenlenir.
                            //coinler setlenir
                            foreach (GameObject coinBar in GetComponent<MainMenu_MenuController>().CoinBarText)
                            {
                                coinBar.GetComponent<CoinBar_Controller>().CoinBarText.text = formattedCloudData.totalCoin.ToString();
                            }

                            //enerji setlenir
                            foreach (GameObject energyBar in GetComponent<EnergyBarController>().EnergyBarText)
                            {
                                energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = formattedCloudData.totalEnergy.ToString();
                            }

                            DateTime unlimitedEnergyEndTime;
                            DateTime.TryParse(formattedCloudData.unlimitedEnergyEndTime, out unlimitedEnergyEndTime);

                            if (formattedCloudData.unlimitedEnergyActive == true )
                            {
                                //s�n�rs�z enerji s�resi devam ediyorsa
                                if (unlimitedEnergyEndTime.CompareTo(DateTime.Now) > 0)
                                {
                                    //s�n�rs�z enerji varsa setlenir.
                                    foreach (GameObject energyBar in GetComponent<EnergyBarController>().EnergyBarText)
                                    {
                                        energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = "\u221E ";
                                        energyBar.GetComponent<EnergyBarProperties>().energyBarText.enableAutoSizing = false;
                                        energyBar.GetComponent<EnergyBarProperties>().energyBarText.fontSize = 55;
                                    }

                                }
                            }
                            #endregion

                        }
                    }
                }
                else//local kay�t dosyas� yoksa
                {
                    formattedCloudData.saveFileIsSyncEver = true;
                    GetComponent<LocalSaveLoadController>().SaveGame(formattedCloudData);
                   
                    #region cloud verisi kaydedildi�i i�in coin bar ve enerji bar save dosayas�na g�re d�zenlenir.
                    //coinler setlenir
                    foreach (GameObject coinBar in GetComponent<MainMenu_MenuController>().CoinBarText)
                    {
                        coinBar.GetComponent<CoinBar_Controller>().CoinBarText.text = formattedCloudData.totalCoin.ToString();
                    }

                    //enerji setlenir
                    foreach (GameObject energyBar in GetComponent<EnergyBarController>().EnergyBarText)
                    {
                        energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = formattedCloudData.totalEnergy.ToString();
                    }

                    DateTime unlimitedEnergyEndTime;
                    DateTime.TryParse(formattedCloudData.unlimitedEnergyEndTime, out unlimitedEnergyEndTime);

                    if (formattedCloudData.unlimitedEnergyActive == true)
                    {
                        //s�n�rs�z enerji s�resi devam ediyorsa
                        if (unlimitedEnergyEndTime.CompareTo(DateTime.Now) > 0)
                        {
                            //s�n�rs�z enerji varsa setlenir.
                            foreach (GameObject energyBar in GetComponent<EnergyBarController>().EnergyBarText)
                            {
                                energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = "\u221E ";
                                energyBar.GetComponent<EnergyBarProperties>().energyBarText.enableAutoSizing = false;
                                energyBar.GetComponent<EnergyBarProperties>().energyBarText.fontSize = 55;
                            }

                        }
                    }
                    #endregion
                }
            }
            else//Cloudda herhangi bir kay�t yoksa
            {
                //local save dosyas� varsa
                if (formattedLocalData.saveTime != null)
                {
                    formattedLocalData.saveFileIsSyncEver = true;
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

    public void CreateDefaultSaveFile()
    {
        SaveDataFormat saveFile = GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime == null)//Kay�tl� save dosyas� yoksa
        {
            saveFile = new SaveDataFormat();

            //default de�er atamalar� burada yap�l�r
            saveFile.totalCoin = GlobalVariables.FirstTotalCoin;
            saveFile.shuffleJokerQuantity = GlobalVariables.FirstShuffleJokerQuantity;
            saveFile.undoJokerQuantity = GlobalVariables.FirstShuffleJokerQuantity;
            saveFile.saveFileIsSyncEver = false;
            saveFile.totalEnergy = GlobalVariables.maxEnergy;
            saveFile.noAdsJokerActive = false;
            saveFile.lastEnergyGainTime = DateTime.Now.ToString();
            saveFile.unlimitedEnergyActive = false;
            saveFile.unlimitedEnergyEndTime = DateTime.Now.ToString();
            saveFile.tutorialPassed = false;

            saveFile.saveTime = DateTime.Now.ToString();

            saveFile.levelProperties = new List<LevelProperties> { new LevelProperties
            {
                LevelName = "level1",
                levelPassed = false,
                levelPurchased = true,
            } };

            GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
        }
    }
}
