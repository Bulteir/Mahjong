using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_Pause_Btn : MonoBehaviour
{
    public void OnClick()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_MainMenu;
        //ge�i�i
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
