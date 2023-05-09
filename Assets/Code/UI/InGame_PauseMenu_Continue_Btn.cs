using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_PauseMenu_Continue_Btn : MonoBehaviour
{
    public void OnClick()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_inGame;
    }
}
