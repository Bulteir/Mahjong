using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetAvailabilityController : MonoBehaviour
{
#if !UNITY_EDITOR
    private void Start()
    {
        InvokeRepeating(nameof(CheckNetwork), 5f, 30.0f);
    }

    public void CheckNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            GlobalVariables.internetAvaible = false;
            GlobalVariables.cloudSaveSystemIsReady = false;
            Debug.Log("internet ba�lant�n�z yok! �nternet ba�lant�n�z olmad��� s�rece oyunun t�m �zelliklerini kullanamazs�n�z.");
        }
        else
        {
            GlobalVariables.internetAvaible = true;
        }

        GetComponent<GooglePlayGameSignIn>().CheckLoginButtonStatus();
        GetComponent<FacebookLogIn>().CheckLoginButtonStatus();

    }
#elif UNITY_EDITOR
    private void Start()
    {

    }

    public void CheckNetwork()
    {
    }

#endif
}
