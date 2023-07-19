using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetAvailabilityControllerInGame : MonoBehaviour
{
    public GameObject InternetRequiredPopup;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(CheckNetwork), 5f, 5.0f);
    }

    public void CheckNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable && GlobalVariables.gameState != GlobalVariables.gameState_inGame)
        {

            GlobalVariables.internetAvaible = false;
            GlobalVariables.cloudSaveSystemIsReady = false;
            InternetRequiredPopup.SetActive(true);
        }
        else if (GlobalVariables.gameState != GlobalVariables.gameState_inGame)
        {
            GlobalVariables.internetAvaible = true;
            InternetRequiredPopup.SetActive(false);
        }
    }
}
