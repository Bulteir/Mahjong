using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground_StartAfterLogo : MonoBehaviour
{
    public GameObject MenuBackground;
    public AudioSource MenuMusic;
    public AudioSource ForestMusic;

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
        Invoke("StartMenuBackgroundAnimations", 3f);
    }

    void StartMenuBackgroundAnimations()
    {
        MenuBackground.SetActive(true);
        gameObject.SetActive(false);
        //MenuMusic.Play();
        ForestMusic.Play();
    }
}
