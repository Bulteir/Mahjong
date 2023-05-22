using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectMenu_Controller : MonoBehaviour
{
    public GameObject generalControllers;
    public Sprite LockImage;
    public Sprite UnlockImage;
    [Tooltip("Oyunda kullan�c� ka� tane b�l�m� a�t�.")]
    int UnlockedLevelNumber = 1;

    LevelSelectMenu_Level_Btn[] levelButtons;

    // Start is called before the first frame update
    void Start()
    {
        //save dosyas� okunur.
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        levelButtons = GetComponentsInChildren<LevelSelectMenu_Level_Btn>();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            LevelProperties levelProperties = new LevelProperties();
            if (saveFile.saveTime != null)//Kay�tl� save dosyas� varsa
            {
                string levelName = "level" + (i + 1).ToString();
                levelProperties = saveFile.levelProperties.Where(i => i.LevelName == levelName).FirstOrDefault();
            }

            //e�er b�l�m kazan�lm��sa unlocked b�l�m say�s�n� artt�r.
            if (levelProperties.bestTime != null)
            {
                UnlockedLevelNumber++;
            }

            levelButtons[i].levelNumberText.text = (i + 1).ToString();
            levelButtons[i].levelNumber = (i+1);

            if (i >= UnlockedLevelNumber)
            {
                levelButtons[i].GetComponent<Image>().sprite = LockImage;
                levelButtons[i].GetComponent<Button>().enabled = false;
                levelButtons[i].levelNumberText.gameObject.SetActive(false);
                levelButtons[i].bestTimeText.gameObject.SetActive(false);
                levelButtons[i].rewardText.gameObject.SetActive(false);
            }
            else
            {
                levelButtons[i].GetComponent<Image>().sprite = UnlockImage;
                levelButtons[i].GetComponent<Button>().enabled = true;
                levelButtons[i].levelNumberText.gameObject.SetActive(true);
                
                //b�l�mler kazan�ln��sa yap�lan en iyi s�renin g�sterilmesi
                if (levelProperties.bestTime != null)
                {
                    levelButtons[i].bestTimeText.gameObject.SetActive(true);
                    levelButtons[i].bestTimeText.text = levelProperties.bestTime;
                }
                else//o b�l�m hi� kazan�lmad��� i�in en iyi s�resi yoktur
                {
                    levelButtons[i].bestTimeText.gameObject.SetActive(false);
                }

                //b�l�mlere g�re kazan�lacak puanlar�n g�sterilmesi
                if (i < GlobalVariables.LevelRewards.Count)
                {
                    levelButtons[i].rewardText.gameObject.SetActive(true);
                    levelButtons[i].rewardText.text = GlobalVariables.LevelRewards[i].ToString();
                }
                else
                {
                    levelButtons[i].rewardText.gameObject.SetActive(false);
                }
            }
        }
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene("level" + levelNumber, LoadSceneMode.Single);
    }
}
