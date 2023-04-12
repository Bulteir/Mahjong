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

        //burada amacýmýz kulklanýcý ana menüye geldiðinde verilerini buluta senkronize etmek. Levelden ana menüye geldiðinde de çalýþýr.
        //oyun ilk açýldýðýnda çalýþmaz. Ýstediðimzide bu. Oyun ilk açýldýðýnda authantication iþlemlerinin bitiminde otomatik çaðrýlýr.
        //Ayrýca bulut hizmetlerinine okuma yazma yükünü hafifletmek için oyun açýldýktan sonra otantike olduðunda bir kez senkronize olur.
        //Bu senkronizasyon haricinde ana menüye bir kez daha geldiðinde son bir kez daha senkronize olur.
        //Yani oyuna baþlayýnca otantike iþlemi bitince birkez senkron olur. sonra oynadýk ana menüye döndük diyelim son bir kez daha senkron olur.
        if (GlobalVariables.cloudSaveSystemIsReady == true && GlobalVariables.firstSynchronisationOfGame == true)
        {
            GetComponent<GameSaveLoadController>().GameSaveDataSynchronization();
            GlobalVariables.firstSynchronisationOfGame = false;
        }
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
