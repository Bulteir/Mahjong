using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_InGame_MenuController : MonoBehaviour
{
    public List<GameObject> CoinBars;
    public List<GameObject> ShuffleJokers;
    public List<GameObject> UndoJokers;
    public Transform musics;
    public Transform sfx;

    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.gameState = GlobalVariables.gameState_tutorial;

        SaveDataFormat saveFile = GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            foreach(GameObject coinBar in CoinBars)
            {
                coinBar.GetComponent<CoinBar_Controller>().CoinBarText.text = saveFile.totalCoin.ToString();
            }

            foreach (GameObject shuffleJoker in ShuffleJokers)
            {
                shuffleJoker.GetComponent<Joker_Shuffle>().QuantityText.text = saveFile.shuffleJokerQuantity.ToString();
            }

            foreach (GameObject undoJoker in UndoJokers)
            {
                undoJoker.GetComponent<Joker_Undo_Controller>().QuantityText.text = saveFile.undoJokerQuantity.ToString();
            }
        }

        #region oyun baþlangýcý muzik tercihi kontrolü
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
        #region oyun baþlangýcý ses efektleri tercihi kontrolü
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

}
