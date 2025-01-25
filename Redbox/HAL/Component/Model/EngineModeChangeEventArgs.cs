using System;


namespace Redbox.HAL.Component.Model
{
    public sealed class EngineModeChangeEventArgs : EventArgs
    {
        public EngineModes NewMode { get; private set; }

        public EngineModeChangeEventArgs(EngineModes newMode) => this.NewMode = newMode;
    }
}
