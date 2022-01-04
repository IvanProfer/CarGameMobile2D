namespace Features.Shed.Upgrade
{
    internal interface IUpgradeHandler
    {
        IUpgradable Upgrade(IUpgradable upgradable);
    }
}
