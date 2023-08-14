using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class SettingsMenu_RestorePurchase_Btn : MonoBehaviour
{
    public GameObject storeController;
    public void OnClick()
    {
        storeController.GetComponent<StoreController>().testConsoleOutput.text = "";
        storeController.GetComponent<StoreController>().RestorePurchase();
    }

}
