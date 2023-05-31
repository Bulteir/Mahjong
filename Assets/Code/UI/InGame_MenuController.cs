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

    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_inGame;

        SaveDataFormat saveFile = GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            CoinBarText.GetComponent<CoinBar_Controller>().CoinBarText.text = saveFile.totalCoin.ToString();
            ShuffleJoker.GetComponent<Joker_Shuffle>().QuantityText.text = saveFile.shuffleJokerQuantity.ToString();
        }

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
