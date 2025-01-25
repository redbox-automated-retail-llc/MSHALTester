using System;


namespace Redbox.HAL.Component.Model.Threading
{
    public struct AtomicFlagHelper : IDisposable
    {
        private bool Disposed;
        private readonly AtomicFlag Flag;

        public void Dispose()
        {
            if (this.Disposed)
                return;
            this.Disposed = true;
            this.Flag.Clear();
        }

        public AtomicFlagHelper(AtomicFlag flag)
          : this()
        {
            this.Disposed = false;
            this.Flag = flag;
        }
    }
}
