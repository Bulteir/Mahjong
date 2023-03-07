using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMP_Text text;
    float count;
    float miliSecond;
    int second;
    int minute;
    int hour;

    bool isStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        startCounter();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isStarted == true && GlobalVariables.gameState == GlobalVariables.gameState_inGame)
        {
            count += Time.deltaTime;
            miliSecond = (count % 1) * 100;
            second = (int)count % 60;
            minute = ((int)count / 60) % 60;
            hour = ((int)count / 3600) % 24;

            if (hour == 0)
                text.text = string.Format("{0:00}:{1:00}:{2:00}", minute, second, (int)miliSecond);
            else
                text.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", hour, minute, second, (int)miliSecond);
        }
    }

    public void startCounter()
    {
        isStarted = true;
    }

    public void pauseCounter()
    {
        isStarted = false;
    }

    public void resetCounter()
    {
        count = 0;
        text.text = string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);

        isStarted = false;
    }
}
