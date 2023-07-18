using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickSFX : MonoBehaviour, IPointerDownHandler
{
    public AudioSource buttonClick;
    // Start is called before the first frame update
    void Start()
    {
        Button button;

        if (this.TryGetComponent<Button>(out button))
        {
            button.onClick.AddListener(PlayButtonSFX);
        }

    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        TMP_Dropdown dropdown;
        Toggle dropdownListItem;

        if (this.TryGetComponent<TMP_Dropdown>(out dropdown))
        {
            PlayButtonSFX();
        }
        else if (this.TryGetComponent<Toggle>(out dropdownListItem))
        {
            PlayButtonSFX();
        }
    }

    void PlayButtonSFX()
    {
        buttonClick.Play();
    }
}
