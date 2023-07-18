using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_Music_Controller : MonoBehaviour
{
    int randomMusicIndex;
    // Start is called before the first frame update
    void Start()
    {
        int randomMusicIndex = Random.Range(0, transform.childCount);
        transform.GetChild(randomMusicIndex).GetComponent<AudioSource>().Play();
    }

    void Update()
    {
        if (transform.GetChild(randomMusicIndex).GetComponent<AudioSource>().isPlaying == false)
        {
            randomMusicIndex++;
            if (randomMusicIndex >= transform.childCount)
                randomMusicIndex = 0;

            transform.GetChild(randomMusicIndex).GetComponent<AudioSource>().Play();
        }

        if (GlobalVariables.gameState == GlobalVariables.gameState_gameOver)
        {
            foreach (Transform music in transform)
            {
                music.GetComponent<AudioSource>().mute = true;
            }
        }
    }

}
