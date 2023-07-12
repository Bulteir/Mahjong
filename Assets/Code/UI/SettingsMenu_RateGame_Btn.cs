using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu_RateGame_Btn : MonoBehaviour
{
    public GameObject RateBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        RateBox.SetActive(true);
    }

}
