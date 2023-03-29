using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelectMenu_Level_Btn : MonoBehaviour
{
    public LevelSelectMenu_Controller levelSelectMenuController;
    public int levelNumber;
    public TMP_Text levelNumberText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        levelSelectMenuController.LoadLevel(levelNumber);
    }
}
