using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu_Sound_Btn : MonoBehaviour
{
    public Transform sounds;
    public Sprite soundOn;
    public Sprite soundOnPressed;
    public Sprite soundOff;
    public Sprite soundOffPressed;

    // Start is called before the first frame update
    void Start()
    {
        string soundPref = PlayerPrefs.GetString("Sound");
        if (soundPref != "")
        {
            if (soundPref == "on")
            {
                this.GetComponent<Image>().sprite = soundOn;

                SpriteState normalPressed = this.GetComponent<Button>().spriteState;
                normalPressed.pressedSprite = soundOnPressed;
                this.GetComponent<Button>().spriteState = normalPressed;
            }
            else
            {
                this.GetComponent<Image>().sprite = soundOff;

                SpriteState OffPressed = this.GetComponent<Button>().spriteState;
                OffPressed.pressedSprite = soundOffPressed;
                this.GetComponent<Button>().spriteState = OffPressed;
            }
        }
    }
    public void Onclick()
    {
        string soundPref = PlayerPrefs.GetString("Sound");
        if (soundPref != "")
        {
            if (soundPref == "on")
            {
                foreach (Transform item in sounds)
                {
                    item.GetComponent<AudioSource>().mute = true;
                }
                this.GetComponent<Image>().sprite = soundOff;

                SpriteState OffPressed = this.GetComponent<Button>().spriteState;
                OffPressed.pressedSprite = soundOffPressed;
                this.GetComponent<Button>().spriteState = OffPressed;

                PlayerPrefs.SetString("Sound", "off");
            }
            else
            {
                foreach (Transform item in sounds)
                {
                    item.GetComponent<AudioSource>().mute = false;
                }
                this.GetComponent<Image>().sprite = soundOn;

                SpriteState normalPressed = this.GetComponent<Button>().spriteState;
                normalPressed.pressedSprite = soundOnPressed;
                this.GetComponent<Button>().spriteState = normalPressed;

                PlayerPrefs.SetString("Sound", "on");
            }
            PlayerPrefs.Save();

        }
    }
}
