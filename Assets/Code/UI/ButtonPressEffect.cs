using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPressEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        Button button;
        TMP_Dropdown dropdown;
        Toggle dropdownListItem;
        if (this.TryGetComponent<Button>(out button))
        {
            this.GetComponentInChildren<TMP_Text>().color = Color.white;
        }
        else if (this.TryGetComponent<TMP_Dropdown>(out dropdown))
        {
            dropdown.placeholder.GetComponent<TMP_Text>().color = Color.white;
            dropdown.captionText.GetComponent<TMP_Text>().color = Color.white;
        }
        else if (this.TryGetComponent<Toggle>(out dropdownListItem))
        {
            this.GetComponentInChildren<TMP_Text>().color = Color.white;
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        //Color PressTextColor = new Color(164.0f / 255, 164.0f / 255, 164.0f / 255);
        Color PressTextColor = new Color(168.0f / 255, 145.0f / 255, 128.0f / 255);
        Button button;
        TMP_Dropdown dropdown;
        Toggle dropdownListItem;

        if (this.TryGetComponent<Button>(out button))
        {
            this.GetComponentInChildren<TMP_Text>().color = PressTextColor;
        }
        else if (this.TryGetComponent<TMP_Dropdown>(out dropdown))
        {
            dropdown.placeholder.GetComponent<TMP_Text>().color = PressTextColor;
            dropdown.captionText.GetComponent<TMP_Text>().color = PressTextColor;
        }
        else if (this.TryGetComponent<Toggle>(out dropdownListItem))
        {
            this.GetComponentInChildren<TMP_Text>().color = PressTextColor;
        }
    }
}
