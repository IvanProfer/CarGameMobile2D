using UnityEngine.Events;

namespace Services.Ads
{
    internal interface IAdsService
    {
        UnityEvent Initialized { get; }

        IAdsPlayer InterstitialPlayer { get; }
        IAdsPlayer RewardedPlayer { get; }
        IAdsPlayer BannerPlayer { get; }
    }
}
