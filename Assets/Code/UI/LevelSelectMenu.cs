using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSelectMenu : MonoBehaviour
{
    [Tooltip("Oyunda toplam kaç bölüm var.")]
    public int totalLevel = 0;
    [Tooltip("Oyunda kaç bölüm açýlmýþ.")]
    public int unlockedLevel = 1;
    [Tooltip("Bir sayfada kaç bölüm gösteriliyor.")]
    public int pageItem = 15;
    public GameObject nextButton;
    public GameObject backButton;

    LevelButton[] levelButtons;
    int totalPage = 0;
    int page = 0;  //þuanki sayfa sayýsý

    void Start()
    {
        levelButtons = gameObject.GetComponentsInChildren<LevelButton>();
        Refresh();
    }

    public void  ClickNext ()
    {
        page++;
        Refresh();
    }

    public void ClickBack()
    {
        page--;
        Refresh();
    }

    public void Refresh ()
    {
        totalPage = (totalLevel-1) / pageItem;
        int index = page * pageItem;
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int level = index + i + 1;

            if (level <= totalLevel)
            {
                levelButtons[i].gameObject.SetActive(true);
                levelButtons[i].Setup(level, level <= unlockedLevel);
            }
            else
            {
                levelButtons[i].gameObject.SetActive(false);
            }
        }
        CheckButton();
    }

    void CheckButton()
    {
        backButton.SetActive(page > 0);
        nextButton.SetActive(page < totalPage);
    }

    public void StartLevel(int level)
    {
        if (level == unlockedLevel)
        {
            unlockedLevel++;
            Refresh();
        }
    }
}
