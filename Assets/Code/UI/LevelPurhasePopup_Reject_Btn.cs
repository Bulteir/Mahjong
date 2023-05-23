using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPurhasePopup_Reject_Btn : MonoBehaviour
{
    public GameObject Popup;
    // Start is called before the first frame update
    public void OnClick()
    {
        Popup.SetActive(false);
    }
}
