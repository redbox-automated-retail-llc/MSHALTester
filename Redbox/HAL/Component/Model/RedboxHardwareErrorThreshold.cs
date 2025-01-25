namespace Redbox.HAL.Component.Model
{
    public sealed class RedboxHardwareErrorThreshold
    {
        private readonly int Threshold;
        private int m_currentCount;

        public int Increment() => ++this.m_currentCount;

        public void Reset() => this.m_currentCount = 0;

        public RedboxHardwareErrorThreshold(int threshold, int current)
        {
            this.Threshold = threshold;
            this.m_currentCount = current;
        }

        public RedboxHardwareErrorThreshold(int threshold)
          : this(threshold, 0)
        {
        }

        public bool Exceeded => this.m_currentCount >= this.Threshold;

        public int CurrentCount => this.m_currentCount;
    }
}
