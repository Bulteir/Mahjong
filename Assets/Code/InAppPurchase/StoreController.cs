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

            Product product = storeController.products.all.Where(i => i.definition.id == item.GetComponent<StoreItem>().Item_Id).FirstOrDefault();
            item.GetComponent<StoreItem>().SetItemMetaData(product.metadata.localizedTitle, product.metadata.localizedDescription, product.metadata.localizedPriceString+" "+ product.metadata.isoCurrencyCode);
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

        return PurchaseProcessingResult.Complete;
    }

    public void Purchase(string productId,Button button)
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

    public string RestorePurchase()
    {
        string result = "sonuç yok";
        if (UnityServices.State != ServicesInitializationState.Initialized || UnityServices.State != ServicesInitializationState.Initializing)
            UnityServicesInitial();

        Product product = storeController.products.all.Where(i => i.definition.id == "no_ads").FirstOrDefault();

        if (product.hasReceipt)
        {
            Debug.Log("Daha önce satýn aldýðýnýz reklam yok ürünü geri getirildi.");
            result = "Daha önce satýn aldýðýnýz reklam yok ürünü geri getirildi.";
        }
        else
        {
            Debug.Log("Reklam yok ürünü satýn alýnmamýþ");
            result = "Reklam yok ürünü satýn alýnmamýþ";

        }
        return result;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        throw new System.NotImplementedException();
    }
}
