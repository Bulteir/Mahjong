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

    //örnek kod. Test butonlarýnda kullanýlýyor
    IEnumerator SaveGameLocal(SaveDataFormat data)
    {
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StartAnimation(LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Saving"));
        yield return null;
        GetComponent<LocalSaveLoadController>().SaveGame(data);
        yield return null;
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StopAnimation();
        yield return null;
    }

    //örnek kod. Test butonlarýnda kullanýlýyor
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
            Debug.Log("Unity Cloud Save servisi hazýr deðil. Veriler buluta yüklenemedi.");
        }
        LoadAnimation.GetComponent<LoadSaveAnimationController>().StopAnimation();
        yield return null;
    }

    SaveDataFormat LoadGameLocal()
    {
        return GetComponent<LocalSaveLoadController>().LoadGame();
    }

    //örnek kod. Test butonlarýnda kullanýlýyor
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

    //örnek kod. Test butonlarýnda kullanýlýyor
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

            //local save dosyasýný okuyoruz.
            SaveDataFormat formattedLocalData = new SaveDataFormat();
            formattedLocalData = LoadGameLocal();

            //Cloudda herhangi bir kayýt varsa
            if (data != "")
            {
                //json cloud datasýný formatlýyoruz
                SaveDataFormat formattedCloudData = JsonConvert.DeserializeObject<SaveDataFormat>(data);

                //local kayýt dosyasý da varsa
                if (formattedLocalData.saveTime != null)
                {
                    if (formattedLocalData.saveFileIsSyncEver == false)//oyun daha önce buluta kaydedilmiþ ve silinip tekrar yüklenmiþse buraya gelir. Ya da cihaz deðiþtirme gibi durumda
                    {
                        SaveFileSyncPopup.GetComponent<SaveFileSyncPopupController>().localSaveFile = formattedLocalData;
                        SaveFileSyncPopup.GetComponent<SaveFileSyncPopupController>().cloudSaveFile = formattedCloudData;
                        SaveFileSyncPopup.GetComponent<SaveFileSyncPopupController>().ShowPopup();
                    }
                    else
                    {
                        //Bu kýsým save dosyalarýnýn sadece kayýt tarihilerini karþýlaþtýrarak en yeni olaný tutarak çalýþýr.
                        DateTime cloudSaveTime;
                        DateTime.TryParse(formattedCloudData.saveTime, out cloudSaveTime);

                        DateTime localSaveTime;
                        DateTime.TryParse(formattedLocalData.saveTime, out localSaveTime);

                        //Local kayýt daha yeni
                        if (cloudSaveTime.CompareTo(localSaveTime) < 0)
                        {
                            //local save dosyasýný buluta atýyoruz
                            GetComponent<CloudSaveController>().SaveData(JsonConvert.SerializeObject(formattedLocalData));
                        }
                        else //Cloud kaydý daha yeni veya localle ayný
                        {
                            //Cloud kaydýný locale kaydediyoruz.
                            GetComponent<LocalSaveLoadController>().SaveGame(formattedCloudData);

                            # region cloud verisi kaydedildiði için coin bar ve enerji bar save dosayasýna göre düzenlenir.
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
                                //sýnýrsýz enerji süresi devam ediyorsa
                                if (unlimitedEnergyEndTime.CompareTo(DateTime.Now) > 0)
                                {
                                    //sýnýrsýz enerji varsa setlenir.
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
                else//local kayýt dosyasý yoksa
                {
                    formattedCloudData.saveFileIsSyncEver = true;
                    GetComponent<LocalSaveLoadController>().SaveGame(formattedCloudData);
                   
                    #region cloud verisi kaydedildiði için coin bar ve enerji bar save dosayasýna göre düzenlenir.
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
                        //sýnýrsýz enerji süresi devam ediyorsa
                        if (unlimitedEnergyEndTime.CompareTo(DateTime.Now) > 0)
                        {
                            //sýnýrsýz enerji varsa setlenir.
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
            else//Cloudda herhangi bir kayýt yoksa
            {
                //local save dosyasý varsa
                if (formattedLocalData.saveTime != null)
                {
                    formattedLocalData.saveFileIsSyncEver = true;
                    //local save dosyasýný buluta atýyoruz
                    GetComponent<CloudSaveController>().SaveData(JsonConvert.SerializeObject(formattedLocalData));
                }
            }
            LoadAnimation.GetComponent<LoadSaveAnimationController>().StopAnimation();
            Debug.Log("Kayýt dosyasý senkronizasyonu tamamlandý.");
        }
        else
        {
            Debug.Log("Kayýt dosyasý senkronizasyonu için unity cloud sistemi hazýr deðil");
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
