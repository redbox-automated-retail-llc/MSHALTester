using System;


namespace Redbox.HAL.Component.Model
{
    public interface ISession : IMessageSink
    {
        void Start();

        bool LogDetailedMessages { get; }

        event EventHandler Disconnect;
    }
}
