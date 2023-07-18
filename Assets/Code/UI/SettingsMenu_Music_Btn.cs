using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu_Music_Btn : MonoBehaviour
{
    public Transform musics;
    public Sprite musicOn;
    public Sprite musicOnPressed;
    public Sprite musicOff;
    public Sprite musicOffPressed;


    // Start is called before the first frame update
    void Start()
    {
        string musicPref = PlayerPrefs.GetString("Music");
        if (musicPref != "")
        {
            if (musicPref == "on")
            {
                this.GetComponent<Image>().sprite = musicOn;

                SpriteState normalPressed = this.GetComponent<Button>().spriteState;
                normalPressed.pressedSprite = musicOnPressed;
                this.GetComponent<Button>().spriteState = normalPressed;
            }
            else
            {
                this.GetComponent<Image>().sprite = musicOff;

                SpriteState OffPressed = this.GetComponent<Button>().spriteState;
                OffPressed.pressedSprite = musicOffPressed;
                this.GetComponent<Button>().spriteState = OffPressed;
            }
        }
    }

    public void Onclick()
    {
        string musicPref = PlayerPrefs.GetString("Music");
        if (musicPref != "")
        {
            if (musicPref == "on")
            {
                foreach (Transform item in musics)
                {
                    item.GetComponent<AudioSource>().mute = true;
                }
                this.GetComponent<Image>().sprite = musicOff;

                SpriteState OffPressed = this.GetComponent<Button>().spriteState;
                OffPressed.pressedSprite = musicOffPressed;
                this.GetComponent<Button>().spriteState = OffPressed;

                PlayerPrefs.SetString("Music", "off");
            }
            else
            {
                foreach (Transform item in musics)
                {
                    item.GetComponent<AudioSource>().mute = false;
                }
                this.GetComponent<Image>().sprite = musicOn;

                SpriteState normalPressed = this.GetComponent<Button>().spriteState;
                normalPressed.pressedSprite = musicOnPressed;
                this.GetComponent<Button>().spriteState = normalPressed;

                PlayerPrefs.SetString("Music", "on");
            }
            PlayerPrefs.Save();
        }
    }
}
