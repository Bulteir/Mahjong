using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground_StartAfterLogo : MonoBehaviour
{
    public GameObject MenuBackground;

    private void Awake()
    {
        if (GlobalVariables.gameState != GlobalVariables.gameState_StartingLogo)
        {
            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartMenuBackgroundAnimations()
    {
        MenuBackground.SetActive(true);
        gameObject.SetActive(false);
    }
}
