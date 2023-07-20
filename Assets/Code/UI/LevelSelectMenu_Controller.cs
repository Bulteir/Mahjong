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
    public Sprite Background;
    public GameObject Popup;
    public Sprite star_On;
    public Sprite star_Off;

    [Tooltip("Oyunda kullanýcý kaç tane bölümü açtý.")]
    int UnlockedLevelNumber = 1;

    LevelSelectMenu_Level_Btn[] levelButtons;

    // Start is called before the first frame update
    void Start()
    {
        CreateLevelSelectMenu();
    }

    public void CreateLevelSelectMenu()
    {
        UnlockedLevelNumber = 1;
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
            if (levelProperties.levelPassed == true)
            {
                UnlockedLevelNumber++;
            }

            levelButtons[i].levelNumberText.text = (i + 1).ToString();
            levelButtons[i].levelNumber = (i + 1);

            if (i >= UnlockedLevelNumber)
            {
                levelButtons[i].GetComponent<Image>().sprite = LockImage;
                levelButtons[i].GetComponent<Button>().enabled = false;
                levelButtons[i].levelNumberText.gameObject.SetActive(false);
                levelButtons[i].bestTimeText.transform.parent.gameObject.SetActive(false);
                levelButtons[i].rewardText.transform.parent.gameObject.SetActive(false);
                levelButtons[i].star1.gameObject.SetActive(false);
                levelButtons[i].star2.gameObject.SetActive(false);
                levelButtons[i].star3.gameObject.SetActive(false);
            }
            else
            {
                //bölüm satýn alýnmýþ yada ilk bölüm
                if ((levelProperties.levelPurchased && i > 0) || i == 0)
                {
                    levelButtons[i].GetComponent<Image>().sprite = Background;
                    levelButtons[i].GetComponent<Button>().enabled = true;

                    levelButtons[i].levelNumberText.gameObject.SetActive(true);

                    //bölümler kazanýlnýþsa yapýlan en iyi sürenin gösterilmesi
                    if (levelProperties.bestTime != null)
                    {
                        levelButtons[i].bestTimeText.transform.parent.gameObject.SetActive(true);
                        levelButtons[i].bestTimeText.text = levelProperties.bestTime;
                    }
                    else//o bölüm hiç kazanýlmadýðý için en iyi süresi yoktur
                    {
                        levelButtons[i].bestTimeText.transform.parent.gameObject.SetActive(false);
                    }

                    //bölümlere göre kazanýlacak puanlarýn gösterilmesi
                    if (i < GlobalVariables.LevelRewards.Count)
                    {
                        levelButtons[i].rewardText.transform.parent.gameObject.SetActive(true);
                        levelButtons[i].rewardText.text = GlobalVariables.LevelRewards[i].ToString();
                    }
                    else
                    {
                        levelButtons[i].rewardText.transform.parent.gameObject.SetActive(false);
                    }

                    //bölüm'e ait kazanýlan yýldýzlarýn gösterilmesi
                    levelButtons[i].star1.gameObject.SetActive(true);
                    levelButtons[i].star2.gameObject.SetActive(true);
                    levelButtons[i].star3.gameObject.SetActive(true);
                    if (levelProperties.earnedStarQuantity == 0)
                    {
                        levelButtons[i].star1.GetComponent<Image>().sprite = star_Off;
                        levelButtons[i].star2.GetComponent<Image>().sprite = star_Off;
                        levelButtons[i].star3.GetComponent<Image>().sprite = star_Off;
                    }
                    else if (levelProperties.earnedStarQuantity == 1)
                    {
                        levelButtons[i].star1.GetComponent<Image>().sprite = star_On;
                        levelButtons[i].star2.GetComponent<Image>().sprite = star_Off;
                        levelButtons[i].star3.GetComponent<Image>().sprite = star_Off;
                    }
                    else if (levelProperties.earnedStarQuantity == 2)
                    {
                        levelButtons[i].star1.GetComponent<Image>().sprite = star_On;
                        levelButtons[i].star2.GetComponent<Image>().sprite = star_On;
                        levelButtons[i].star3.GetComponent<Image>().sprite = star_Off;
                    }
                    else if (levelProperties.earnedStarQuantity == 3)
                    {
                        levelButtons[i].star1.GetComponent<Image>().sprite = star_On;
                        levelButtons[i].star2.GetComponent<Image>().sprite = star_On;
                        levelButtons[i].star3.GetComponent<Image>().sprite = star_On;
                    }
                }
                else if (i > 0)//bölüm satýn alýnmamýþ
                {
                    levelButtons[i].GetComponent<Image>().sprite = UnlockImage;
                    levelButtons[i].GetComponent<Button>().enabled = true;
                    levelButtons[i].levelNumberText.gameObject.SetActive(false);
                    levelButtons[i].bestTimeText.transform.parent.gameObject.SetActive(false);
                    levelButtons[i].rewardText.transform.parent.gameObject.SetActive(false);
                    levelButtons[i].star1.gameObject.SetActive(false);
                    levelButtons[i].star2.gameObject.SetActive(false);
                    levelButtons[i].star3.gameObject.SetActive(false);
                }
            }
        }
    }

    public void LoadLevel(int levelNumber)
    {
        //save dosyasý okunur.
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        LevelProperties levelProperties = new LevelProperties();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            string levelName = "level" + levelNumber.ToString();
            levelProperties = saveFile.levelProperties.Where(i => i.LevelName == levelName).FirstOrDefault();
        }

        //Oyuncu 1. leveli týkladýðýnda daha önce tutoriali görmediyse önce tutorial açýlýr.
        if (levelNumber == 1 && saveFile.tutorialPassed == false)
        {
            SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
        }
        else
        {
            //týklanýlan bölüm daha önceden satýn alýnmýþsa ya da ilk bölümse
            if (levelProperties.levelPurchased || levelNumber == 1)
            {
                if (generalControllers.GetComponent<EnergyBarController>().IsThereEnoughEnergyForLevel())
                {
                    SceneManager.LoadScene("level" + levelNumber, LoadSceneMode.Single);
                }
            }
            else
            {
                Popup.GetComponent<LevelPurhasePopup_Controller>().ShowPopup(saveFile, levelNumber);
            }
        }
    }
}
