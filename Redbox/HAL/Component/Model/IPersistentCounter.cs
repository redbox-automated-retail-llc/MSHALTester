namespace Redbox.HAL.Component.Model
{
    public interface IPersistentCounter
    {
        int Increment();

        int Decrement();

        void Reset();

        string Name { get; }

        int Value { get; }
    }
}
