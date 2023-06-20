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

    public void NotEnoughCoinAnim()
    {
        StartCoroutine(vibrationAnimation());
    }

    IEnumerator vibrationAnimation()
    {
        Vector3 startPos = new Vector3();

        startPos = CoinBarText.GetComponent<RectTransform>().localPosition;

        for (int i = 0; i < 6; i++)
        {
            if (i % 2 == 0)
            {

                CoinBarText.GetComponent<RectTransform>().localPosition = new Vector3(startPos.x + 10, startPos.y, startPos.z);
            }
            else
            {

                CoinBarText.GetComponent<RectTransform>().localPosition = new Vector3(startPos.x - 10, startPos.y, startPos.z);

            }
            yield return new WaitForSeconds(0.05f);
        }

        CoinBarText.GetComponent<RectTransform>().localPosition = new Vector3(startPos.x, startPos.y, startPos.z);

        yield return null;
    }
}
