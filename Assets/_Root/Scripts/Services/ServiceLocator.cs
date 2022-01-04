using Services.IAP;
using Services.Ads;
using Services.Ads.UnityAds;
using Services.Analytics;
using UnityEngine;

namespace Services
{
    internal class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator _instance;
        private static ServiceLocator Instance => _instance ??= FindObjectOfType<ServiceLocator>();

        public static IAdsService AdsService => _instance._adsService;
        public static IIAPService IAPService => _instance._iapService;
        public static IAnalyticsManager Analytics => _instance._analyticsManager;

        [SerializeField] private UnityAdsService _adsService;
        [SerializeField] private IAPService _iapService;
        [SerializeField] private AnalyticsManager _analyticsManager;


        private void Awake() =>
            _instance = this;
    }
}
