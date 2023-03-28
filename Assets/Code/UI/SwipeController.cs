using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwipeController : MonoBehaviour
{
    [Tooltip("Ne kadar uzunlukta swipe iþlemi yapýldýktan sonra algýlansýn.")]
    public float SwipeDetectThreshold = 0;

    public UnityEvent LeftSwipe;
    public UnityEvent RightSwipe;

    Vector2 startTouchPosition;
    Vector2 endTouchPosition;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;


            if (startTouchPosition.x - endTouchPosition.x > SwipeDetectThreshold )
            {
                //next
                RightSwipe.Invoke();
            }

            if(endTouchPosition.x - startTouchPosition.x > SwipeDetectThreshold)
            {
                //back
                LeftSwipe.Invoke();
            }
        }
    }
}
