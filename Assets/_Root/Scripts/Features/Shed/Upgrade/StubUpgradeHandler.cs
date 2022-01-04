namespace Features.Shed.Upgrade
{
    internal class StubUpgradeHandler : IUpgradeHandler
    {
        public static readonly IUpgradeHandler Default = new StubUpgradeHandler();

        public IUpgradable Upgrade(IUpgradable upgradable) => upgradable;
    }

}
