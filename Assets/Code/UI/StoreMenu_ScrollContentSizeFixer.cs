using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreMenu_ScrollContentSizeFixer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        SetContentHeight();
    }

    void SetContentHeight()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, Mathf.Abs(transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition.y) + transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>().sizeDelta.y+50);
    }
}
