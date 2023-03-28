using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_StartGame_Btn : MonoBehaviour
{
    public void OnClick ()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_LevelSelectmenu;
    }
}
