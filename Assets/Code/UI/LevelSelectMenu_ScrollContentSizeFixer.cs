using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSelectMenu_ScrollContentSizeFixer : MonoBehaviour
{
    public Canvas mainCanvas;
    public List<GameObject> pages;
    [Tooltip("Sayfalar aras�nda ne kadar bo�luk olsun.")]
    public float spacing;
    // Start is called before the first frame update
    void Start()
    {
        SetPageRect();
        SetContentWidht();
    }

    void SetContentWidht()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2((pages.Count* (mainCanvas.GetComponent<RectTransform>().rect.width+spacing))-spacing, GetComponent<RectTransform>().sizeDelta.y);
    }

    void SetPageRect()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            //sayfan�n geni�li�i setleniyor.
            pages[i].GetComponent<RectTransform>().sizeDelta = new Vector2(mainCanvas.GetComponent<RectTransform>().rect.width, pages[i].GetComponent<RectTransform>().sizeDelta.y);
            //sayfan�n ba�lang�� koordinat� setleniyor
            if (i > 0)
            {
                pages[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((i * (mainCanvas.GetComponent<RectTransform>().rect.width + spacing)) , 0);
            }
        }
    }
}
