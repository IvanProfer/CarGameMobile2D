namespace Services.Ads.UnityAds
{
    internal class EmptyPlayer : UnityAdsPlayer
    {
        public EmptyPlayer(string id) : base(id)
        { }

        protected override void OnPlaying()
        { }

        protected override void Load()
        { }
    }
}
