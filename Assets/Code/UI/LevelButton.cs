using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public TMP_Text LevelButtonNumberText;
    public Sprite LockImage;
    public LevelSelectMenu _LevelSelectMenu;
    int level;

    public void Setup (int level, bool isUnlock)
    {
        this.level = level;
        LevelButtonNumberText.text = level.ToString();

        if (isUnlock)
        {
            gameObject.GetComponent<Image>().sprite = null;
            gameObject.GetComponent<Button>().enabled = true;
            LevelButtonNumberText.gameObject.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = LockImage;
            gameObject.GetComponent<Button>().enabled = false;
            LevelButtonNumberText.gameObject.SetActive(false);
        }
    }

    public void OnClick()
    {
        _LevelSelectMenu.StartLevel(level);
    }
}
