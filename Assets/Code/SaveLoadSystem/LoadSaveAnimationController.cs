using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadSaveAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartAnimation(string Text)
    {
        GetComponentInChildren<TMP_Text>().text = Text;
        gameObject.SetActive(true);
        GetComponent<Animator>().enabled = true;
    }

    public void StopAnimation()
    {
        gameObject.SetActive(false);
        GetComponent<Animator>().enabled = false;
    }
}
