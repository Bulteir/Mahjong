using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_NextLevelInfoPopup_Confirm_Btn : MonoBehaviour
{
    public GameObject NextLevelInfoPopup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        NextLevelInfoPopup.SetActive(false);
    }
}
