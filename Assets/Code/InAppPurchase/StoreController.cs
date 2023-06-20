using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

public class StoreController : MonoBehaviour, IDetailedStoreListener
{
    public GameObject PurchasingMask;
    public GameObject generalControllers;
    public List<GameObject> StoreItems;
    private IStoreController storeController;
    private IExtensionProvider storeExtensions;
    private Button purchaseButton;


    void Start()
    {
        UnityServicesInitial();
    }

    async void UnityServicesInitial()
    {
        InitializationOptions options = new InitializationOptions()
#if UNITY_EDITOR || DEVELOPMENT_BUILD
.SetEnvironmentName("Test");
#else
            .SetEnvironmentName("Production");
#endif

        if (UnityServices.State != ServicesInitializationState.Initialized || UnityServices.State != ServicesInitializationState.Initializing)
            await UnityServices.InitializeAsync(options);

        LoadCatalog();
    }

    void LoadCatalog()
    {
        ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((Resources.LoadAsync<TextAsset>("IAPProductCatalog").asset as TextAsset).text);

        //StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
        //StandardPurchasingModule.Instance().useFakeStoreAlways = true;

#if UNITY_ANDROID
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));
#elif UNITY_IOS
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.AppleAppStore));
#else
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.NotSpecified));
#endif

        foreach (ProductCatalogItem item in catalog.allProducts)
        {
            builder.AddProduct(item.id, item.type);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    //IAP baþarýlý þekilde baþlatýldýðýnda girer
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.storeController = controller;
        this.storeExtensions = extensions;
        foreach (GameObject item in StoreItems)
        {
            //ProductCatalogItem itemData = catalog.allProducts.Where(i => i.id == item.GetComponent<StoreItem>().Item_Id).FirstOrDefault();

            Product product_ = storeController.products.all.Where(i => i.definition.id == item.GetComponent<StoreItem>().Item_Id).FirstOrDefault();
            item.GetComponent<StoreItem>().SetItemMetaData(product_.metadata.localizedTitle, product_.metadata.localizedDescription, product_.metadata.localizedPriceString + " " + product_.metadata.isoCurrencyCode);
        }
    }

    //IAP baþlatýlmasýnda hata ile karþýlaþýlýrsa
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new System.NotImplementedException();
    }

    //IAP baþlatýlmasýnda hata ile karþýlaþýlýrsa
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Error initializing IAP because of {error}. \r\nError message: {message}");
    }

    //Satýn almada hata meydana gelirse
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Failed to purchase {product.definition.id} because {failureReason}");
        PurchaseCompleted();
        PurchasingMask.SetActive(false);
    }

    //satýn alma baþarýlý olursa
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}");
        PurchaseCompleted();
        PurchasingMask.SetActive(false);

        //do something, like give the player their currency, unlock the item 
        //update some metrics or analytics, etc...

        ApplyPurchasedItemEffect(purchaseEvent.purchasedProduct.definition.id, purchaseEvent.purchasedProduct.metadata.localizedDescription);

        return PurchaseProcessingResult.Complete;
    }

    public void Purchase(string productId, Button button)
    {
        purchaseButton = button;
        purchaseButton.enabled = false;
        PurchasingMask.SetActive(true);
        Product product = storeController.products.all.Where(i => i.definition.id == productId).FirstOrDefault();
        storeController.InitiatePurchase(product);
    }

    private void PurchaseCompleted()
    {
        purchaseButton.enabled = true;
    }

    public async void RestorePurchase()
    {

        InitializationOptions options = new InitializationOptions()
#if UNITY_EDITOR || DEVELOPMENT_BUILD
.SetEnvironmentName("Test");
#else
            .SetEnvironmentName("Production");
#endif

        if (UnityServices.State != ServicesInitializationState.Initialized || UnityServices.State != ServicesInitializationState.Initializing)
            await UnityServices.InitializeAsync(options);

        LoadCatalog();

        if (storeController != null)
        {
            Product product = storeController.products.all.Where(i => i.definition.id == "no_ads").FirstOrDefault();
            if (product != null)
            {
                if (product.hasReceipt)
                {
                    SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
                    if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
                    {
                        saveFile.noAdsJokerActive = true;
                    }
                    saveFile.saveTime = DateTime.Now.ToString();
                    generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);

                    Debug.Log("Daha önce satýn aldýðýnýz reklam yok ürünü geri getirildi.");
                }
                else
                {
                    Debug.Log("Reklam yok ürünü satýn alýnmamýþ");
                }
            }
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {

        Debug.Log("Satýn almada hata meydana geldi. message:" + failureDescription.message + " reason:" + failureDescription.reason + " ýd:" + failureDescription.productId);

        if (product.hasReceipt)
        {
            Debug.Log("Daha önce satýn alýnmýþ. id:" + product.definition.id);
            ApplyPurchasedItemEffect(product.definition.id, product.metadata.localizedDescription);
        }

        PurchaseCompleted();
        PurchasingMask.SetActive(false);
    }

    //maðazadan bir ürün satýn alýndýðýnda oyuna nasýl etki edecekse ayarlamalar yapýlýr.
    void ApplyPurchasedItemEffect(string itemId, string description)
    {
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            if (itemId == "no_ads")
            {
                saveFile.noAdsJokerActive = true;
            }
            else if (itemId.Contains("coin"))
            {
                saveFile.totalCoin += Int32.Parse(description);

                foreach (GameObject coinBar in generalControllers.GetComponent<MainMenu_MenuController>().CoinBarText)
                {
                    coinBar.GetComponent<CoinBar_Controller>().AddRemoveCoin(Int32.Parse(description), saveFile.totalCoin);
                }
            }
            else if (itemId.Contains("joker_shuffle"))
            {
                saveFile.shuffleJokerQuantity += Int32.Parse(description);
            }
            else if (itemId.Contains("joker_undo"))
            {
                saveFile.undoJokerQuantity += Int32.Parse(description);
            }
            else if (itemId.Contains("energy_unlimited_"))
            {
                saveFile.unlimitedEnergyActive = true;


                DateTime unlimitedEnergyEndTime;
                DateTime.TryParse(saveFile.unlimitedEnergyEndTime, out unlimitedEnergyEndTime);

                //limitsiz enerji süresi bitmediyse
                if (unlimitedEnergyEndTime.CompareTo(DateTime.Now) > 0)
                {
                    saveFile.unlimitedEnergyEndTime = unlimitedEnergyEndTime.AddHours(Int32.Parse(description)).ToString();
                }
                else
                {
                    saveFile.unlimitedEnergyEndTime = DateTime.Now.AddHours(Int32.Parse(description)).ToString();
                } 

                foreach (GameObject energyBar in generalControllers.GetComponent<EnergyBarController>().EnergyBarText)
                {
                    energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = "\u221E ";
                    energyBar.GetComponent<EnergyBarProperties>().energyBarText.enableAutoSizing = false;
                    energyBar.GetComponent<EnergyBarProperties>().energyBarText.fontSize = 55;
                }
            }
            else if (itemId.Contains("energy"))
            {
                saveFile.totalEnergy += Int32.Parse(description);
                foreach (GameObject energyBar in generalControllers.GetComponent<EnergyBarController>().EnergyBarText)
                {
                    energyBar.GetComponent<EnergyBarProperties>().energyBarText.text = saveFile.totalEnergy.ToString();
                }
            }

            saveFile.saveTime = DateTime.Now.ToString();
            generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
        }

    }
}
