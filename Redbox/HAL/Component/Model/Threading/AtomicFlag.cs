using System.Threading;


namespace Redbox.HAL.Component.Model.Threading
{
    public sealed class AtomicFlag
    {
        private int m_flagState;
        private readonly int Flagged;
        private readonly int Unflagged;
        private const int DefaultFlagged = 1;
        private const int DefaultUnflagged = 0;

        public bool Set()
        {
            return Interlocked.CompareExchange(ref this.m_flagState, this.Flagged, this.Unflagged) == this.Unflagged;
        }

        public bool Clear()
        {
            return Interlocked.CompareExchange(ref this.m_flagState, this.Unflagged, this.Flagged) == this.Flagged;
        }

        public AtomicFlag(bool initial)
          : this(initial, 1, 0)
        {
        }

        public AtomicFlag()
          : this(false, 1, 0)
        {
        }

        public AtomicFlag(bool initial, int flagged, int unflagged)
        {
            this.Flagged = flagged;
            this.Unflagged = unflagged;
            Interlocked.Exchange(ref this.m_flagState, initial ? this.Flagged : this.Unflagged);
        }

        public bool IsSet => Thread.VolatileRead(ref this.m_flagState) == this.Flagged;
    }
}
