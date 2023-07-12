using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBackground_DestroyObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public void DestroyObjectAfterAnimation()
    {
        Destroy(gameObject);
    }

    public void ShowMainMenuAfterAnimation()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_MainMenu;

    }
}
