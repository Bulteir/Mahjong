using Newtonsoft.Json;
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
            Debug.Log("Yerel kay�t dosyas� null. Kay�t dosyas� buluta g�nderilemedi.");
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
            Debug.Log("Bulut kay�t dosyas� null. Bulut kay�t dosyas� al�namad�.");
        }
        else
        {
            cloudSaveFile.saveFileIsSyncEver = true;
            generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(cloudSaveFile);
        }
        gameObject.SetActive(false);
    }
}
