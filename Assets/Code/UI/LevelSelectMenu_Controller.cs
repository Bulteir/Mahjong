using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectMenu_Controller : MonoBehaviour
{
    public Sprite LockImage;
    [Tooltip("Oyunda kullanýcý kaç tane bölümü açtý.")]
    public int UnlockedLevelNumber = 1;

    LevelSelectMenu_Level_Btn[] levelButtons;

    // Start is called before the first frame update
    void Start()
    {
        levelButtons = GetComponentsInChildren<LevelSelectMenu_Level_Btn>();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].levelNumberText.text = (i + 1).ToString();
            levelButtons[i].levelNumber = (i+1);

            if (i >= UnlockedLevelNumber)
            {
                levelButtons[i].GetComponent<Image>().sprite = LockImage;
                levelButtons[i].GetComponent<Button>().enabled = false;
                levelButtons[i].levelNumberText.gameObject.SetActive(false);
            }
            else
            {
                levelButtons[i].GetComponent<Image>().sprite = null;
                levelButtons[i].GetComponent<Button>().enabled = true;
                levelButtons[i].levelNumberText.gameObject.SetActive(true);
            }
        }
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene("level" + levelNumber, LoadSceneMode.Single);
    }


}
