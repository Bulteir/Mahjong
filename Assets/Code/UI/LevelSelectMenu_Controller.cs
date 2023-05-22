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
    [Tooltip("Oyunda kullanýcý kaç tane bölümü açtý.")]
    int UnlockedLevelNumber = 1;

    LevelSelectMenu_Level_Btn[] levelButtons;

    // Start is called before the first frame update
    void Start()
    {
        //save dosyasý okunur.
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        levelButtons = GetComponentsInChildren<LevelSelectMenu_Level_Btn>();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            LevelProperties levelProperties = new LevelProperties();
            if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
            {
                string levelName = "level" + (i + 1).ToString();
                levelProperties = saveFile.levelProperties.Where(i => i.LevelName == levelName).FirstOrDefault();
            }

            //eðer bölüm kazanýlmýþsa unlocked bölüm sayýsýný arttýr.
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
                
                //bölümler kazanýlnýþsa yapýlan en iyi sürenin gösterilmesi
                if (levelProperties.bestTime != null)
                {
                    levelButtons[i].bestTimeText.gameObject.SetActive(true);
                    levelButtons[i].bestTimeText.text = levelProperties.bestTime;
                }
                else//o bölüm hiç kazanýlmadýðý için en iyi süresi yoktur
                {
                    levelButtons[i].bestTimeText.gameObject.SetActive(false);
                }

                //bölümlere göre kazanýlacak puanlarýn gösterilmesi
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
