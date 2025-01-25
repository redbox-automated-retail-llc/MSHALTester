using System;


namespace Redbox.HAL.Component.Model
{
    public interface IIPCChannel : IDisposable
    {
        byte[] Read();

        byte[] Read(int timeout);

        void Read(IIPCResponse response);

        void Read(IIPCResponse response, int timeout);

        bool Write(byte[] b);

        bool Connect();

        bool Disconnect();

        bool IsConnected { get; }
    }
}
