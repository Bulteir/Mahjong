using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardMenu_Back_Btn : MonoBehaviour
{
    public void OnClick()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_MainMenu;
    }
}
