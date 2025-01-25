namespace Redbox.HAL.Component.Model
{
    public interface IIpcServiceHost
    {
        void Start();

        void Stop();

        void Register(ISession session);

        void Unregister(ISession session);

        bool Alive { get; }

        IIpcProtocol Protocol { get; }

        IHostInfo HostInfo { get; }
    }
}
