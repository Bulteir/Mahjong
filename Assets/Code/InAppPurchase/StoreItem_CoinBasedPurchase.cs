using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem_CoinBasedPurchase : MonoBehaviour
{
    public GameObject generalControllers;
    public string Item_Id;
    public GameObject StoreController;
    public Button PurchaseButton;
    public TMP_Text PriceText;
    public TMP_Text ContentText;
    public int price;

    public AudioSource successfulPurchaseSound;

    // Start is called before the first frame update
    void Start()
    {
        PriceText.text = price.ToString();
    }

    public void OnClick()
    {
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            
            if (saveFile.totalCoin >= price)
            {
                saveFile.totalCoin -= price;

                foreach (GameObject coinbar in generalControllers.GetComponent<MainMenu_MenuController>().CoinBarText)
                {
                    if (coinbar.activeInHierarchy)
                    {
                        coinbar.GetComponent<CoinBar_Controller>().AddRemoveCoin(-price, saveFile.totalCoin);
                    }
                    else
                    {
                        coinbar.GetComponent<CoinBar_Controller>().CoinBarText.text = saveFile.totalCoin.ToString();
                    }
                }

                if (Item_Id == "coinBasedPurchase_shuffle")
                {
                    saveFile.shuffleJokerQuantity += 1;
                }
                else if (Item_Id == "coinBasedPurchase_undo")
                {
                    saveFile.undoJokerQuantity += 1;
                }
                else if (Item_Id == "coinBasedPurchase_unlimitedEnergy")
                {
                    saveFile.unlimitedEnergyActive = true;

                    DateTime unlimitedEnergyEndTime;
                    DateTime.TryParse(saveFile.unlimitedEnergyEndTime, out unlimitedEnergyEndTime);

                    //limitsiz enerji süresi bitmediyse
                    if (unlimitedEnergyEndTime.CompareTo(DateTime.Now) > 0)
                    {
                        saveFile.unlimitedEnergyEndTime = unlimitedEnergyEndTime.AddMinutes(30).ToString();
                    }
                    else
                    {
                        saveFile.unlimitedEnergyEndTime = DateTime.Now.AddMinutes(30).ToString();
                    }

                    foreach (GameObject energyBar in generalControllers.GetComponent<EnergyBarController>().EnergyBarText)
                    {
                        energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = "\u221E ";
                        energyBar.GetComponent<EnergyBarProperties>().energyBarText.enableAutoSizing = false;
                        energyBar.GetComponent<EnergyBarProperties>().energyBarText.fontSize = 55;
                    }
                }

                saveFile.saveTime = DateTime.Now.ToString();
                generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
                successfulPurchaseSound.Play();
            }
            else
            {
                foreach (GameObject coinbar in generalControllers.GetComponent<MainMenu_MenuController>().CoinBarText)
                {
                    if (coinbar.activeInHierarchy)
                    {
                        coinbar.GetComponent<CoinBar_Controller>().NotEnoughCoinAnim();
                    }
                    else
                    {
                        coinbar.GetComponent<CoinBar_Controller>().CoinBarText.text = saveFile.totalCoin.ToString();
                    }
                }
            }
        }
    }
}
