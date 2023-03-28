using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{
    [Tooltip("Oyunda toplam kaç bölüm var.")]
    public int totalLevel = 0;
    [Tooltip("Oyunda kaç bölüm açýlmýþ.")]
    public int unlockedLevel = 1;
    [Tooltip("Bir sayfada kaç bölüm gösteriliyor.")]
    public int pageItem = 15;
    public TMP_Text pageNumberText;

    LevelButton[] levelButtons;
    int totalPage = 0;
    int page = 0;  //þuanki sayfa sayýsý
    bool nextPageAvaible = false;
    bool previousPageAvaible = false;

    void Start()
    {
        levelButtons = gameObject.GetComponentsInChildren<LevelButton>();
        Refresh();
    }

    public void Refresh ()
    {
        totalPage = (totalLevel-1) / pageItem;
        int index = page * pageItem;
        pageNumberText.text = (page+1) + "/" + (totalPage+1);
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
        CheckPagesAvailability();
    }

    public void StartLevel(int level)
    {
        ////týklanýlan bölüm kilitli olmayan son bölüm ise
        //if (level == unlockedLevel)
        //{
        //    unlockedLevel++;
        //    Refresh();
        //}

        SceneManager.LoadScene("level"+level, LoadSceneMode.Single);
        //unlockedLevel++;

    }

    void CheckPagesAvailability()
    {
        nextPageAvaible = page < totalPage;
        previousPageAvaible = page > 0;
    }

    public void NextPage()
    {
        if (nextPageAvaible)
        {
            page++;
            Refresh();
        }
    }

    public void PreviousPage()
    {
        if (previousPageAvaible)
        {
            page--;
            Refresh();
        }
    }
}
