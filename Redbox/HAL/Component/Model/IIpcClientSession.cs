using System;
using System.Collections.Generic;


namespace Redbox.HAL.Component.Model
{
    public interface IIpcClientSession : IDisposable
    {
        void ConnectThrowOnError();

        bool IsStatusOK(List<string> messages);

        List<string> ExecuteCommand(string command);

        int Timeout { get; set; }

        bool IsConnected { get; }

        IIpcProtocol Protocol { get; }

        bool IsDisposed { get; }

        event Action<string> ServerEvent;
    }
}
