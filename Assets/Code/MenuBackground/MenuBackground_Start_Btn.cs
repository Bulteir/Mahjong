using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground_Start_Btn : MonoBehaviour
{
    public GameObject menuBackgroundController;
    public List<GameObject> animatedButtons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        gameObject.SetActive(false);
        menuBackgroundController.GetComponent<MenuBackground_Controller>().AnimStart = true;
        foreach (GameObject button in animatedButtons)
        {
            button.GetComponent<Animator>().enabled = true;
        }
    }
}
