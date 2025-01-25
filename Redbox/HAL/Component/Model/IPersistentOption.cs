namespace Redbox.HAL.Component.Model
{
    public interface IPersistentOption
    {
        void UpdateValue(string value);

        string Key { get; }

        string Value { get; }
    }
}
