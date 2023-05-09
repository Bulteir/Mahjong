using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_MenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_inGame;
    }

    // Update is called once per frame
    void Update()
    {
        menuActiveControl();
    }

    void menuActiveControl()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_inGame)
        {
            pauseMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_gamePaused && pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);
        }
    }
}
