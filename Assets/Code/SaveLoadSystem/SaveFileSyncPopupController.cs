using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class SaveFileSyncPopupController : MonoBehaviour
{
    public SaveDataFormat localSaveFile;
    public SaveDataFormat cloudSaveFile;
    public GameObject generalControllers;
    public TMP_Text localSaveInfo;
    public TMP_Text cloudSaveInfo;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ShowPopup()
    {
        Dictionary<string, string> arguments = new Dictionary<string, string> { { "saveTime", localSaveFile.saveTime } };
        localSaveInfo.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Save File Sync Local Info", new object[] { arguments });

        Dictionary<string, string> arguments2 = new Dictionary<string, string> { { "saveTime", cloudSaveFile.saveTime } };
        cloudSaveInfo.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Save File Sync Cloud Info", new object[] { arguments2 });
        gameObject.SetActive(true);
    }

    public void SelectLocalSaveFile()
    {
        if (localSaveFile == null)
        {
            Debug.Log("Yerel kayýt dosyasý null. Kayýt dosyasý buluta gönderilemedi.");
        }
        else
        {
            localSaveFile.saveFileIsSyncEver = true;
            generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(localSaveFile);
            generalControllers.GetComponent<CloudSaveController>().SaveData(JsonConvert.SerializeObject(localSaveFile));
        }
        gameObject.SetActive(false);
    }

    public void SelectCloudSaveFile()
    {
        if (cloudSaveFile == null)
        {
            Debug.Log("Bulut kayýt dosyasý null. Bulut kayýt dosyasý alýnamadý.");
        }
        else
        {
            cloudSaveFile.saveFileIsSyncEver = true;
            generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(cloudSaveFile);

            #region cloud verisi kaydedildiði için coin bar ve enerji bar save dosayasýna göre düzenlenir.
            //coinler setlenir
            foreach (GameObject coinBar in generalControllers.GetComponent<MainMenu_MenuController>().CoinBarText)
            {
                coinBar.GetComponent<CoinBar_Controller>().CoinBarText.text = cloudSaveFile.totalCoin.ToString();
            }

            //enerji setlenir
            foreach (GameObject energyBar in generalControllers.GetComponent<EnergyBarController>().EnergyBarText)
            {
                energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = cloudSaveFile.totalEnergy.ToString();
            }

            DateTime unlimitedEnergyEndTime;
            DateTime.TryParse(cloudSaveFile.unlimitedEnergyEndTime, out unlimitedEnergyEndTime);

            if (cloudSaveFile.unlimitedEnergyActive == true)
            {
                //sýnýrsýz enerji süresi devam ediyorsa
                if (unlimitedEnergyEndTime.CompareTo(DateTime.Now) > 0)
                {
                    //sýnýrsýz enerji varsa setlenir.
                    foreach (GameObject energyBar in generalControllers.GetComponent<EnergyBarController>().EnergyBarText)
                    {
                        energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = "\u221E ";
                        energyBar.GetComponent<EnergyBarProperties>().energyBarText.enableAutoSizing = false;
                        energyBar.GetComponent<EnergyBarProperties>().energyBarText.fontSize = 55;
                    }

                }
            }
            #endregion
        }
        gameObject.SetActive(false);
    }
}
