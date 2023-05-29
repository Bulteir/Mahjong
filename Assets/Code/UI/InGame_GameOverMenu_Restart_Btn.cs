using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_GameOverMenu_Restart_Btn : MonoBehaviour
{
    public GameObject generalControllers;
    public void OnClick()
    {
        if (generalControllers.GetComponent<EnergyBarController>().IsThereEnoughEnergyForLevel())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }
}
