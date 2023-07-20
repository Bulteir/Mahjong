using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public List<GameObject> Items;
    public GameObject blocks;
    public AudioSource ButtonClick;
    int step;
    // Start is called before the first frame update
    void Start()
    {
        step = 1;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            NextStepWithMouse();
        }
        else
        {
            NextStepWithTouch();
        }

    }

    void NextStepWithTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (step < Items.Count)
                {
                    if (step == 1)
                    {
                        blocks.transform.localPosition = new Vector3(blocks.transform.localPosition.x, blocks.transform.localPosition.y, 0);
                    }

                    Items[step].gameObject.SetActive(true);
                    if (step > 0)
                    {
                        Items[step - 1].gameObject.SetActive(false);
                    }
                    step++;
                    ButtonClick.Play();

                }
            }
        }
    }

    void NextStepWithMouse()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            if (step < Items.Count)
            {
                if (step == 1)
                {
                    blocks.transform.localPosition = new Vector3 (blocks.transform.localPosition.x, blocks.transform.localPosition.y, 150);
                }

                Items[step].gameObject.SetActive(true);
                if (step > 0)
                {
                    Items[step-1].gameObject.SetActive(false);
                }
                step++;
                ButtonClick.Play();
            }
            
        }
    }
}
