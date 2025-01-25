namespace Redbox.HAL.Component.Model
{
    public interface IDeviceDescriptor
    {
        bool ResetDriver();

        bool MatchDriver();

        bool Locate();

        DeviceStatus GetStatus();

        string Vendor { get; }

        string Product { get; }

        string Friendlyname { get; }

        IDeviceSetupClass SetupClass { get; }
    }
}
