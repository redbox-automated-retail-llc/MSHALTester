namespace Redbox.HAL.Component.Model
{
    public interface IComponentErrorThreshold
    {
        bool Correct();

        int Increment();

        void Reset();

        bool Exceeded { get; }
    }
}
