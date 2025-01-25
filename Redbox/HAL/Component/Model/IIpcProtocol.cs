namespace Redbox.HAL.Component.Model
{
    public interface IIpcProtocol
    {
        string GetPipeName();

        bool IsSecure { get; }

        ChannelType Channel { get; }

        string Host { get; }

        string Port { get; }

        string Scheme { get; }
    }
}
