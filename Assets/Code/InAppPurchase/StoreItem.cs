using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    public string Item_Id;
    public GameObject StoreController;
    public Button PurchaseButton;
    public TMP_Text PriceText;
    public TMP_Text DescriptionText;
    public TMP_Text TitleText;
    public TMP_Text ContentText;

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
        //ideal olan IAP servisten gelen title ve description'ý göstermek ancak bunun localizationu nasýl idere edilir bilmediðimiz için kullanmýyoruz.
        //TitleText.text = Title;
        //DescriptionText.text = Description;
        PriceText.text = Price;

        if (Item_Id.Contains("coin"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "coin", Description} };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "IAP_Coin", new object[] { arguments });
        }
        else if (Item_Id == "no_ads")
        {
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "IAP_NoAds");
        }
        else if (Item_Id.Contains("energy_unlimited_1"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "hour", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "IAP_energy_unlimited1", new object[] { arguments });
        }
        else if (Item_Id.Contains("energy_unlimited_2"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "day", "1" } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "IAP_energy_unlimited2", new object[] { arguments });
        }
        else if (Item_Id.Contains("energy_unlimited_3"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "day", "7" } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "IAP_energy_unlimited3", new object[] { arguments });
        }
        else if (Item_Id.Contains("energy"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "energy", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "IAP_energy", new object[] { arguments });
        }
        else if (Item_Id.Contains("joker_shuffle"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "quantity", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "IAP_Joker_Shuffle", new object[] { arguments });
        }
        else if (Item_Id.Contains("joker_undo"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "quantity", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizedTextTable", "IAP_Joker_Undo", new object[] { arguments });
        }

    }
}
