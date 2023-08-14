using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu_LoginButtonsIOSAlignment : MonoBehaviour
{
    public GameObject GoogleLoginButton;
    public GameObject FacebookLoginButton;
    private void OnEnable()
    {
#if UNITY_IOS
        GoogleLoginButton.SetActive(false);
        FacebookLoginButton.GetComponent<RectTransform>().anchoredPosition = new Vector2 (0, FacebookLoginButton.GetComponent<RectTransform>().anchoredPosition.y);
#endif
    }
}
