using System;


namespace Redbox.HAL.Component.Model
{
    public interface IIPCResponse : IDisposable
    {
        bool Accumulate(byte[] rawResponse);

        bool Accumulate(byte[] bytes, int start, int length);

        void Clear();

        bool IsComplete { get; }
    }
}
