using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectMenu_ScrollView : MonoBehaviour
{
    public GameObject scrollbar;
    float scrollPos = 0;
    float[] pos;

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_LevelSelectmenu)
        {

            pos = new float[transform.childCount];
            float distance = 1f / (pos.Length - 1);
            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = distance * i;
            }

            if (Input.GetMouseButton(0))
            {
                scrollPos = scrollbar.GetComponent<Scrollbar>().value;
            }
            else
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                    {
                        scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                    }
                }
            }

            #region bu k�s�m se�ili olan item d���ndaki itemler� smooth �ekilde k���lt�r. Ekran�n ortas�na geleni b�y�t�r.
            //for (int i = 0; i < pos.Length; i++)
            //{
            //    if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            //    {
            //        transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
            //        for (int j = 0; j < pos.Length; j++)
            //        {
            //            if (j != i)
            //            {
            //                transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);

            //            }
            //        }

            //    }
            //}
            #endregion
        }

    }
}
