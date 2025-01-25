using System.Collections.Generic;


namespace Redbox.HAL.Component.Model
{
    public interface ICameraPlugin
    {
        bool Start();

        void Snap(string file);

        bool Stop();

        void InitWithProperties(IDictionary<string, object> properties);

        bool IsRunning { get; }

        bool SupportsReset { get; }
    }
}
