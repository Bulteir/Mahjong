using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground_Controller : MonoBehaviour
{
    public GameObject ZoomIn;
    public bool AnimStart = false;
    float animationDuration = 16;
    //float animationDuration = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AnimStart)
        {
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, ZoomIn.transform.localScale.x, animationDuration * Time.deltaTime), transform.localScale.y, transform.localScale.z);
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, ZoomIn.transform.localScale.y, animationDuration * Time.deltaTime), transform.localScale.z);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition.x, ZoomIn.GetComponent<RectTransform>().anchoredPosition.x, animationDuration * Time.deltaTime), Mathf.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition.y, ZoomIn.GetComponent<RectTransform>().anchoredPosition.y, animationDuration * Time.deltaTime));
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Lerp(gameObject.GetComponent<RectTransform>().sizeDelta.x, ZoomIn.GetComponent<RectTransform>().sizeDelta.x, animationDuration * Time.deltaTime), Mathf.Lerp(gameObject.GetComponent<RectTransform>().sizeDelta.y, ZoomIn.GetComponent<RectTransform>().sizeDelta.y, animationDuration * Time.deltaTime));

            //animasyon bittiðinde menüyü gösterir
            if (Mathf.Abs(gameObject.GetComponent<RectTransform>().anchoredPosition.x - ZoomIn.GetComponent<RectTransform>().anchoredPosition.x) < 0.1f)
            {
                AnimStart = false;
            }
        }
    }
}
