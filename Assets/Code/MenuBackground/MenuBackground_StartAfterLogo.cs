using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground_StartAfterLogo : MonoBehaviour
{
    public GameObject MenuBackground;
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
