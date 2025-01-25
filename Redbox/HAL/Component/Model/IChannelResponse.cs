using System;


namespace Redbox.HAL.Component.Model
{
    public interface IChannelResponse : IDisposable
    {
        int GetIndex(byte b);

        bool CommOk { get; }

        ErrorCodes Error { get; }

        byte[] RawResponse { get; }

        bool ReponseValid { get; }
    }
}
