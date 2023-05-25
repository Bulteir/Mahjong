using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinBar_Controller : MonoBehaviour
{
    public TMP_Text CoinBarText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddRemoveCoin(int changedValue, int totalCoin)
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ValueChangeAnimation(changedValue, totalCoin));
        }
        else
        {
            CoinBarText.text = totalCoin.ToString();
        }
    }

    IEnumerator ValueChangeAnimation(int changedValue, int totalCoin)
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

        CoinBarText.text = text;
        yield return new WaitForSeconds(2);

        CoinBarText.text = totalCoin.ToString();
        yield return null;
    }
}
