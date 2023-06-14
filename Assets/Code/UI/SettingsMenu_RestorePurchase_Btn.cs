using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsMenu_RestorePurchase_Btn : MonoBehaviour
{
    public GameObject storeController;
    public void OnClick()
    {
        storeController.GetComponent<StoreController>().RestorePurchase();
    }
}
