using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class InGame_GameOverMenu_NextLevel_Btn : MonoBehaviour
{
    public GameObject generalControllers;
    public GameObject nextLevelInfoPopup;
    public void OnClick()
    {
        LevelProperties levelProperties = new LevelProperties();
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            //bir sonraki bölümün numarasý
            int levelNumber = Int32.Parse(SceneManager.GetActiveScene().name.Replace("level", "")) + 1;
            //bir sonraki bölümün adý
            string levelName = "level" + levelNumber.ToString();
            levelProperties = saveFile.levelProperties.Where(i => i.LevelName == levelName).FirstOrDefault();


            if (levelProperties.LevelName != null && levelProperties.levelPurchased) //bir sonraki bölüm daha önceden satýn alýnmýþ.
            {
                SceneManager.LoadScene("level" + levelNumber, LoadSceneMode.Single);
            }
            else if (saveFile.totalCoin >= GlobalVariables.LevelRewards[levelNumber-1] * 3)//bölümü satýn almak için yeterli altýn var
            {
                saveFile.totalCoin = saveFile.totalCoin - (GlobalVariables.LevelRewards[levelNumber - 1] * 3);

                saveFile.levelProperties.Remove(levelProperties);

                //açýlacak bölüme ait bir kayýt yoktur.
                if (levelProperties.LevelName == null)
                {
                    levelProperties.LevelName = "level" + levelNumber.ToString();
                    levelProperties.levelPassed = false;
                }

                levelProperties.levelPurchased = true;
                saveFile.levelProperties.Add(levelProperties);

                generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);

                generalControllers.GetComponent<InGame_MenuController>().CoinBarText.GetComponent<CoinBar_Controller>().AddRemoveCoin(-(GlobalVariables.LevelRewards[levelNumber - 1] * 3), saveFile.totalCoin);
            }
            else//yeterli altýn yok
            {
                nextLevelInfoPopup.GetComponent<InGame_NextLevelInfoPopup_Controller>().ShowPopup(levelNumber);

                //Dictionary<string, string> arguments = new Dictionary<string, string> { { "levelName", LevelNumber.ToString() } };
                //Popup.GetComponent<LevelPurhasePopup_Controller>().Content.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Level Purchase Not Enough Gold", new object[] { arguments });
                //gameObject.SetActive(false);
            }
        }

    }
}
