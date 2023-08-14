using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetAvailabilityController : MonoBehaviour
{

    public GameObject InternetRequiredPopup;

    void Start()
    {
        InvokeRepeating(nameof(CheckNetwork), 5f, 5.0f);
    }

    public void CheckNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {

            GlobalVariables.internetAvaible = false;
            GlobalVariables.cloudSaveSystemIsReady = false;
            InternetRequiredPopup.SetActive(true);
        }
        else
        {
            GlobalVariables.internetAvaible = true;
            InternetRequiredPopup.SetActive(false);
        }
#if UNITY_ANDROID
        GetComponent<GooglePlayGameSignIn>().CheckLoginButtonStatus();
#endif
        GetComponent<FacebookLogIn>().CheckLoginButtonStatus();

    }
}
