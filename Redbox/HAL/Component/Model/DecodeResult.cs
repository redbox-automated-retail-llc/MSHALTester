namespace Redbox.HAL.Component.Model
{
    public sealed class DecodeResult : IDecodeResult
    {
        private int m_count = 1;

        public string Matrix { get; private set; }

        public int Count => this.m_count;

        public void IncrementCount() => ++this.m_count;

        public DecodeResult(string matrix) => this.Matrix = matrix;
    }
}
