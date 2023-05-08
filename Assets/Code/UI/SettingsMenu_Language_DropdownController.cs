using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsMenu_Language_DropdownController : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;

    // Start is called before the first frame update
    public void ChangeSelected()
    {
        int selectedItemVal = languageDropdown.value;
        if (selectedItemVal == 0)//arapça
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            PlayerPrefs.SetString("SelectedLang", "0");
            ReloadMenu();
        }
        else if (selectedItemVal == 1)//çince
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            PlayerPrefs.SetString("SelectedLang", "1");
            ReloadMenu();
        }
        else if (selectedItemVal == 2)//english
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
            PlayerPrefs.SetString("SelectedLang", "2");
            ReloadMenu();
        }
        else if (selectedItemVal == 3)//fransýzca
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[3];
            PlayerPrefs.SetString("SelectedLang", "3");
            ReloadMenu();
        }
        else if (selectedItemVal == 4)//almanca
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[4];
            PlayerPrefs.SetString("SelectedLang", "4");
            ReloadMenu();
        }
        else if (selectedItemVal == 5)//japonca
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[5];
            PlayerPrefs.SetString("SelectedLang", "5");
            ReloadMenu();
        }
        else if (selectedItemVal == 6)//portekizce
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[6];
            PlayerPrefs.SetString("SelectedLang", "6");
            ReloadMenu();
        }
        else if (selectedItemVal == 7)//rusça
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[7];
            PlayerPrefs.SetString("SelectedLang", "7");
            ReloadMenu();
        }
        else if (selectedItemVal == 8)//ispanyolca
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[8];
            PlayerPrefs.SetString("SelectedLang", "8");
            ReloadMenu();
        }
        else if (selectedItemVal == 9)//türkçe
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[9];
            PlayerPrefs.SetString("SelectedLang", "9");
            ReloadMenu();
        }
        PlayerPrefs.Save();
    }

    void ReloadMenu()
    {
        //SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
