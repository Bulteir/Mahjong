using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBarController : MonoBehaviour
{
    public List<GameObject> EnergyBarText;

    // Start is called before the first frame update
    void Start()
    {
        //test için 10 sn
        //InvokeRepeating(nameof(UpdateEnergyBar), 10f, 10.0f);
        //6dk'da 1
        InvokeRepeating(nameof(UpdateEnergyBar), 360f, 360f);
        EnergyBarInit();
    }

    void EnergyBarInit()
    {
        SaveDataFormat saveFile = GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            if (saveFile.totalEnergy < GlobalVariables.maxEnergy)
            {
                DateTime lastEnergyGain;
                DateTime.TryParse(saveFile.lastEnergyGainTime, out lastEnergyGain);

                if (lastEnergyGain.CompareTo(DateTime.Now) < 0)
                {
                    int gainedEnergy = (int)(DateTime.Now - lastEnergyGain).TotalMinutes / 6;
                    //test için
                    //int gainedEnergy = (int)((DateTime.Now - lastEnergyGain).TotalMinutes) * 2;

                    if (gainedEnergy > 0)
                    {
                        saveFile.totalEnergy += gainedEnergy;

                        if (saveFile.totalEnergy > GlobalVariables.maxEnergy)
                        {
                            saveFile.totalEnergy = GlobalVariables.maxEnergy;
                        }

                        saveFile.lastEnergyGainTime = DateTime.Now.ToString();

                        foreach (GameObject energyBar in EnergyBarText)
                        {
                            energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = saveFile.totalEnergy.ToString();
                        }

                        GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
                    }
                    else
                    {
                        foreach (GameObject energyBar in EnergyBarText)
                        {
                            energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = saveFile.totalEnergy.ToString();
                        }
                    }

                }
            }
        }
        else
        {
            foreach (GameObject energyBar in EnergyBarText)
            {
                energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = GlobalVariables.maxEnergy.ToString();
            }
        }
    }

    void UpdateEnergyBar()
    {
        SaveDataFormat saveFile = GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            if (saveFile.totalEnergy < GlobalVariables.maxEnergy)
            {
                saveFile.totalEnergy++;
                saveFile.lastEnergyGainTime = DateTime.Now.ToString();

                StartCoroutine(ValueChangeAnimation(1, saveFile.totalEnergy));

                GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
            }
        }
        else
        {
            foreach (GameObject energyBar in EnergyBarText)
            {
                energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = GlobalVariables.maxEnergy.ToString();
            }
        }
    }

    public bool IsThereEnoughEnergyForLevel()
    {
        bool result = false;
        SaveDataFormat saveFile = GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            if (saveFile.totalEnergy >= GlobalVariables.requiredEnergyForlevel)
            {
                saveFile.totalEnergy -= GlobalVariables.requiredEnergyForlevel;
                GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
                StartCoroutine(ValueChangeAnimation(-GlobalVariables.requiredEnergyForlevel, saveFile.totalEnergy));
                result = true;
            }
            else
            {
                StartCoroutine(vibrationAnimation());
            }
        }

        return result;
    }

    IEnumerator ValueChangeAnimation(int changedValue, int totalEnergy)
    {
        string text = "";

        if (changedValue < 0)
        {
            text = changedValue.ToString();
        }
        else
        {
            text = "+" + changedValue.ToString();
        }

        foreach (GameObject energyBar in EnergyBarText)
        {
            energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = text;
        }
        yield return new WaitForSeconds(2);

        foreach (GameObject energyBar in EnergyBarText)
        {
            energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = totalEnergy.ToString();
        }
        yield return null;
    }

    IEnumerator vibrationAnimation()
    {
        List<Vector3> startPos = new List<Vector3>();
        foreach (GameObject energyBar in EnergyBarText)
        {
            startPos.Add(energyBar.GetComponent<RectTransform>().localPosition);
        }

        for (int i = 0; i < 6; i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j <  EnergyBarText.Count; j++)
                {
                    EnergyBarText[j].GetComponent<RectTransform>().localPosition = new Vector3(startPos[j].x + 10, startPos[j].y, startPos[j].z);
                }
            }
            else
            {
                for (int j = 0; j < EnergyBarText.Count; j++)
                {
                    EnergyBarText[j].GetComponent<RectTransform>().localPosition = new Vector3(startPos[j].x - 10, startPos[j].y, startPos[j].z);
                }
            }
            yield return new WaitForSeconds(0.05f);
        }

        for (int j = 0; j < EnergyBarText.Count; j++)
        {
            EnergyBarText[j].GetComponent<RectTransform>().localPosition = new Vector3(startPos[j].x , startPos[j].y, startPos[j].z);
        }

        yield return null;
    }
}
