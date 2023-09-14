using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu_LoginButtonsIOSAlignment : MonoBehaviour
{
    public GameObject GoogleLoginButton;
    public GameObject FacebookLoginButton;
    public GameObject AppleGameCenterLoginButton;
    private void OnEnable()
    {
#if UNITY_IOS
        GoogleLoginButton.SetActive(false);
        AppleGameCenterLoginButton.SetActive(true);
#elif UNITY_ANDROID
        GoogleLoginButton.SetActive(true);
        AppleGameCenterLoginButton.SetActive(false);
#endif
    }
}
