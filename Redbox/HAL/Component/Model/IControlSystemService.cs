namespace Redbox.HAL.Component.Model
{
    public interface IControlSystemService
    {
        void AddHandler(IControlSystemObserver observer);

        void RemoveHandler(IControlSystemObserver observer);

        bool Restart();

        bool IsInitialized { get; }
    }
}
