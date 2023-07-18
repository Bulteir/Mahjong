using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGame_MenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject CoinBarText;
    public GameObject ShuffleJoker;
    public GameObject UndoJoker;
    public Transform musics;
    public Transform sfx;

    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_inGame;

        SaveDataFormat saveFile = GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kay�tl� save dosyas� varsa
        {
            CoinBarText.GetComponent<CoinBar_Controller>().CoinBarText.text = saveFile.totalCoin.ToString();
            ShuffleJoker.GetComponent<Joker_Shuffle>().QuantityText.text = saveFile.shuffleJokerQuantity.ToString();
            UndoJoker.GetComponent<Joker_Undo_Controller>().QuantityText.text = saveFile.undoJokerQuantity.ToString();
        }

        #region oyun ba�lang�c� muzik tercihi kontrol�
        string musicPref = PlayerPrefs.GetString("Music");
        if (musicPref != "")
        {
            if (musicPref == "on")
            {
                foreach (Transform item in musics)
                {
                    item.GetComponent<AudioSource>().mute = false;
                }
            }
            else
            {
                foreach (Transform item in musics)
                {
                    item.GetComponent<AudioSource>().mute = true;
                }
            }
        }

        #endregion
        #region oyun ba�lang�c� ses efektleri tercihi kontrol�
        string soundPref = PlayerPrefs.GetString("Sound");
        if (soundPref != "")
        {
            if (soundPref == "on")
            {
                foreach (Transform item in sfx)
                {
                    item.GetComponent<AudioSource>().mute = false;
                }
            }
            else
            {
                foreach (Transform item in sfx)
                {
                    item.GetComponent<AudioSource>().mute = true;
                }
            }
        }
        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        menuActiveControl();
    }

    void menuActiveControl()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_inGame)
        {
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_gamePaused && pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);
            gameOverMenu.SetActive(false);
        }
        else if (GlobalVariables.gameState == GlobalVariables.gameState_gameOver && gameOverMenu.activeSelf == false)
        {
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(true);
        }
    }
}
