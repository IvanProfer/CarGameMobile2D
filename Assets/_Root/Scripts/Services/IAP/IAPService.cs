using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace Services.IAP
{
    internal interface IIAPService : IStoreListener
    {
        UnityEvent Initialized { get; }
        UnityEvent PurchaseSucceed { get; }
        UnityEvent PurchaseFailed { get; }


        void Buy(string id);
        void RestorePurchases();
        string GetCost(string productID);
    }

    internal class IAPService : MonoBehaviour, IIAPService
    {
        [Header("Components")]
        [SerializeField] private ProductLibrary productLibrary;

        [field: Header("Events")]
        [field: SerializeField] public UnityEvent Initialized { get; private set; }
        [field: SerializeField] public UnityEvent PurchaseSucceed { get; private set; }
        [field: SerializeField] public UnityEvent PurchaseFailed { get; private set; }

        private bool _isInitialized;
        private IStoreController _controller;
        private IExtensionProvider _extensionProvider;
        private PurchaseValidator _purchaseValidator;
        private PurchaseRestorer _purchaseRestorer;


        private void Awake() =>
            InitializeProducts();

        private void InitializeProducts()
        {
            StandardPurchasingModule purchasingModule = StandardPurchasingModule.Instance();
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(purchasingModule);

            foreach (Product product in productLibrary.Products)
                builder.AddProduct(product.Id, product.ProductType);

            Log("Products initialized");
            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensionsProvider)
        {
            _isInitialized = true;
            _controller = controller;
            _extensionProvider = extensionsProvider;
            _purchaseValidator = new PurchaseValidator();
            _purchaseRestorer = new PurchaseRestorer(_extensionProvider);

            Log("Initialized");
            Initialized?.Invoke();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            _isInitialized = false;
            Error("Initialization Failed");
        }


        public void Buy(string id)
        {
            if (!_isInitialized)
            {
                Error($"Buy {id} FAIL. Not initialized.");
                return;
            }

            _controller.InitiatePurchase(id);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            if (_purchaseValidator.Validate(args) == false)
            {
                OnPurchaseFailed(args.purchasedProduct.definition.id, "NonValid");
                return PurchaseProcessingResult.Complete;
            }

            OnPurchaseSucceed(args.purchasedProduct);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason) =>
            OnPurchaseFailed(product.definition.id, failureReason.ToString());

        private void OnPurchaseFailed(string productId, string reason)
        {
            Error($"Failed {productId}: {reason}");
            PurchaseFailed?.Invoke();
        }

        private void OnPurchaseSucceed(UnityEngine.Purchasing.Product product)
        {
            string productId = product.definition.id;
            decimal amount = (decimal)product.definition.payout.quantity;
            string currency = product.metadata.isoCurrencyCode;
            ServiceLocator.Analytics.SendTransaction(productId, amount, currency);

            Log($"Purchased: {productId}");
            PurchaseSucceed.Invoke();
        }

        public string GetCost(string productID)
        {
            UnityEngine.Purchasing.Product product = _controller.products.WithID(productID);

            if (product != null)
                return product.metadata.localizedPriceString;

            return "N/A";
        }

        public void RestorePurchases()
        {
            if (!_isInitialized)
            {
                Error("RestorePurchases FAIL. Not initialized.");
                return;
            }

            _purchaseRestorer.Restore();
        }


        private void Log(string message) => Debug.Log(WrapMessage(message));
        private void Error(string message) => Debug.LogError(WrapMessage(message));
        private string WrapMessage(string message) => $"[{GetType().Name}] {message}";
    }
}
