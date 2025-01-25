using System;
using System.Threading;


namespace Redbox.HAL.Component.Model.Threading
{
    public sealed class CountdownEvent : IDisposable
    {
        private readonly ManualResetEvent Event = new ManualResetEvent(false);
        private readonly int m_initialCount;
        private int m_currentCount;
        private bool m_disposed;

        public void Dispose()
        {
            if (this.m_disposed)
                return;
            this.m_disposed = true;
            this.Event.Close();
        }

        public void Signal()
        {
            if (Interlocked.Decrement(ref this.m_currentCount) != 0)
                return;
            this.Event.Set();
        }

        public void Wait() => this.Event.WaitOne();

        public CountdownEvent(int waiterCount)
        {
            this.m_initialCount = this.m_currentCount = waiterCount;
        }

        public bool IsSet => Thread.VolatileRead(ref this.m_currentCount) <= 0;
    }
}
