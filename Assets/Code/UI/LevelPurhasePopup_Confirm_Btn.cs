using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LevelPurhasePopup_Confirm_Btn : MonoBehaviour
{
    public GameObject Popup;
    public GameObject generalControllers;
    public GameObject levelSelectMenu;
    SaveDataFormat SaveFile;
    int LevelNumber;

    // Start is called before the first frame update
    public void OnClick()
    {
        LevelProperties levelProperties = new LevelProperties();
        if (SaveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            string levelName = "level" + LevelNumber.ToString();
            levelProperties = SaveFile.levelProperties.Where(i => i.LevelName == levelName).FirstOrDefault();


            //bölümü satýn almak için yeterli altýn var
            if (SaveFile.totalCoin >= GlobalVariables.LevelRewards[LevelNumber - 1] * 3)
            {
                SaveFile.totalCoin = SaveFile.totalCoin - (GlobalVariables.LevelRewards[LevelNumber - 1] * 3);

                SaveFile.levelProperties.Remove(levelProperties);

                //açýlacak bölüme ait bir kayýt yoktur.
                if (levelProperties.LevelName == null)
                {
                    levelProperties.LevelName = levelName;
                    levelProperties.levelPassed = false;
                }

                levelProperties.levelPurchased = true;
                SaveFile.levelProperties.Add(levelProperties);

                generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(SaveFile);

                Popup.SetActive(false);
                levelSelectMenu.GetComponent<LevelSelectMenu_Controller>().CreateLevelSelectMenu();

                foreach (TMP_Text coinBar in generalControllers.GetComponent<MainMenu_MenuController>().CoinBarText)
                {
                    coinBar.text = SaveFile.totalCoin.ToString();
                }
            }
            else//yeterli altýn yok
            {
                Dictionary<string, string> arguments = new Dictionary<string, string> { { "levelName", LevelNumber.ToString() } };
                Popup.GetComponent<LevelPurhasePopup_Controller>().Content.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "Level Purchase Not Enough Gold", new object[] { arguments });
                gameObject.SetActive(false);
            }
        }
    }

    public void SetConfirmButton(SaveDataFormat saveFile,int levelNumber)
    {
        SaveFile = saveFile;
        LevelNumber = levelNumber;
        gameObject.SetActive(true);
    }
}
