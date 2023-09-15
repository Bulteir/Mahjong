using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardedAdResultController : MonoBehaviour
{

    public Sprite EarnedRewardCoinImage;
    public Sprite EarnedRewardShuffleImage;
    public Sprite EarnedRewardUndoImage;
    public TMP_Text RewardText;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ShowEarnedReward(string rewardAmount)
    {
        gameObject.SetActive(true);
        GetComponent<RectTransform>().localPosition = new Vector2(0, 566);
        RewardText.text = "+" + rewardAmount;
        if (GlobalVariables.MainMenuRewardAdType == GlobalVariables.MainMenuRewardAdType_Coin)
        {
            gameObject.GetComponent<Image>().sprite = EarnedRewardCoinImage;
        }
        else if (GlobalVariables.MainMenuRewardAdType == GlobalVariables.MainMenuRewardAdType_Shuffle)
        {
            gameObject.GetComponent<Image>().sprite = EarnedRewardShuffleImage;
        }
        else if (GlobalVariables.MainMenuRewardAdType == GlobalVariables.MainMenuRewardAdType_Undo)
        {
            gameObject.GetComponent<Image>().sprite = EarnedRewardUndoImage;
        }

        StartCoroutine(EarnedRewardAnimation());
    }

    IEnumerator EarnedRewardAnimation()
    {
        yield return new WaitForSeconds(1);

        Vector2 pos = GetComponent<RectTransform>().localPosition;
        while (GetComponent<RectTransform>().localPosition.y - pos.y < 400)
        {
            transform.Translate(Vector3.up * 1 * Time.deltaTime);
            yield return null;
        }
        yield return null;
        gameObject.SetActive(false);

    }

}
