using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    public string Item_Id;
    public GameObject StoreController;
    public Button PurchaseButton;
    public TMP_Text PriceText;
    public TMP_Text DescriptionText;
    public TMP_Text TitleText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        StoreController.GetComponent<StoreController>().Purchase(Item_Id, PurchaseButton);
        Debug.Log("Satýn alma iþlemi baþladý");
    }

    public void SetItemMetaData(string Title, string Description, string Price)
    {
        TitleText.text = Title;
        PriceText.text = Price;
        DescriptionText.text = Description;
    }
}
