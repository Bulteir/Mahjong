using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsMenu_RestorePurchase_Btn : MonoBehaviour
{
    public GameObject storeController;
    public TMP_Text console;
    public void OnClick()
    {   
        console.text = storeController.GetComponent<StoreController>().RestorePurchase();
    }
}
