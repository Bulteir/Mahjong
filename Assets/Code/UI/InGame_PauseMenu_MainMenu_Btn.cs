using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_PauseMenu_MainMenu_Btn : MonoBehaviour
{
    public void OnClick()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_MainMenu;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
