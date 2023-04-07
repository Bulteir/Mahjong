using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelectMenu;

    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        menuActiveControl();
    }

    // Update is called once per frame
    void Update()
    {
        menuActiveControl();
    }

    void menuActiveControl()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_MainMenu && mainMenu.activeSelf == false)
        {
            mainMenu.SetActive(true);
            levelSelectMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_LevelSelectmenu && levelSelectMenu.activeSelf == false)
        {
            mainMenu.SetActive(false);
            levelSelectMenu.SetActive(true);
        }
    }
}
